Imports System
Imports DevExpress.Xpo
Imports System.Threading
Imports DevExpress.ExpressApp
Imports DevExpress.Xpo.Metadata
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Data.Filtering
Imports System.Collections.Generic
Imports DevExpress.Xpo.DB.Exceptions
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Xpo.DB

Namespace GenerateUserFriendlyId.Module
    'Dennis: This class is used to generate sequential numbers for persistent objects.
    'Use the GetNextSequence method to get the next number and the Accept method, to save these changes to the database.
    Public Class SequenceGenerator
        Implements IDisposable

        Public Const MaxGenerationAttemptsCount As Integer = 10
        Public Const MinGenerationAttemptsDelay As Integer = 100
'INSTANT VB TODO TASK: There is no VB equivalent to 'volatile':
'ORIGINAL LINE: private static volatile IDataLayer defaultDataLayer;

        Private Shared defaultDataLayer_Renamed As IDataLayer
        Private Shared syncRoot As New Object()
        Private euow As ExplicitUnitOfWork
        Private seq As Sequence
        Public Sub New(ByVal lockedSequenceTypes As Dictionary(Of String, Boolean))
            Dim count As Integer = MaxGenerationAttemptsCount
            Do
                Try
                    euow = New ExplicitUnitOfWork(DefaultDataLayer)
                    'Dennis: It is necessary to update all sequences because objects graphs may be complex enough, and so their sequences should be locked to avoid a deadlock.
                    Dim sequences As New XPCollection(Of Sequence)(euow, New InOperator("TypeName", lockedSequenceTypes.Keys), New SortProperty("TypeName", SortingDirection.Ascending))
                    For Each seq As Sequence In sequences
                        seq.Save()
                    Next seq
                    euow.FlushChanges()
                    Exit Do
                Catch e1 As LockingException
                    Close()
                    count -= 1
                    If count <= 0 Then
                        Throw
                    End If
                    Thread.Sleep(MinGenerationAttemptsDelay * count)
                End Try
            Loop
        End Sub
        Public Sub Accept()
            euow.CommitChanges()
        End Sub
        Public Sub Close()
            If euow IsNot Nothing Then
                If euow.InTransaction Then
                    euow.RollbackTransaction()
                End If
                euow.Dispose()
                euow = Nothing
            End If
        End Sub
        Public Sub Dispose() Implements IDisposable.Dispose
            Close()
        End Sub
        Public Function GetNextSequence(ByVal theObject As Object) As Long
            Guard.ArgumentNotNull(theObject, "theObject")
            Return GetNextSequence(XafTypesInfo.Instance.FindTypeInfo(theObject.GetType()))
        End Function
        Public Function GetNextSequence(ByVal typeInfo As ITypeInfo) As Long
            Guard.ArgumentNotNull(typeInfo, "typeInfo")
            Return GetNextSequence(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary.GetClassInfo(typeInfo.Type))
        End Function
        Public Function GetNextSequence(ByVal classInfo As XPClassInfo) As Long
            Return GetNextSequence(GetBaseSequenceName(classInfo))
        End Function
        Public Function GetNextSequence(ByVal name As String) As Long
            seq = euow.GetObjectByKey(Of Sequence)(name, True)
            If seq Is Nothing Then
                'throw new InvalidOperationException(string.Format("Sequence for the {0} type was not found.", name));
                seq = CreateSequece(euow, name)
            End If
            Dim nextSequence As Long = seq.NextSequence
            seq.NextSequence += 1
            euow.FlushChanges()
            Return nextSequence
        End Function
        Public Shared Function GetBaseSequenceName(ByVal classInfo As XPClassInfo) As String
            Guard.ArgumentNotNull(classInfo, "classInfo")
            Dim ci As XPClassInfo = classInfo
            'Comment this code if you want to have the SequentialNumber column created in each derived class table.
            Do While ci.BaseClass IsNot Nothing AndAlso ci.BaseClass.IsPersistent
                ci = ci.BaseClass
            Loop
            Return ci.FullName
        End Function
        Private Shared Function CreateSequece(ByVal uow As UnitOfWork, ByVal typeName As String) As Sequence
            Dim seq As New Sequence(uow)
            seq.TypeName = typeName
            seq.NextSequence = 1
            Return seq
        End Function
        'Dennis: It is necessary to generate (only once) sequences for all the persistent types before using the GetNextSequence method.
        Public Shared Sub RegisterSequences(ByVal persistentTypes As IEnumerable(Of ITypeInfo))
            If persistentTypes IsNot Nothing Then
                Using uow As New UnitOfWork(DefaultDataLayer)
                    Dim sequenceList As New XPCollection(Of Sequence)(uow)
                    Dim typeToExistsMap As New Dictionary(Of String, Boolean)()
                    For Each seq As Sequence In sequenceList
                        typeToExistsMap(seq.TypeName) = True
                    Next seq
                    For Each typeInfo As ITypeInfo In persistentTypes
                        Dim ti As ITypeInfo = typeInfo
                        If typeToExistsMap.ContainsKey(ti.FullName) Then
                            Continue For
                        End If
                        'Comment this code if you want to have the SequentialNumber column created in each derived class table.
                        Do While ti.Base IsNot Nothing AndAlso ti.Base.IsPersistent
                            ti = ti.Base
                        Loop
                        Dim typeName As String = ti.FullName
                        'Dennis: This code is required for the Domain Components only.
                        If ti.IsInterface AndAlso ti.IsPersistent Then
                            Dim generatedEntityType As Type = XpoTypesInfoHelper.GetXpoTypeInfoSource().GetGeneratedEntityType(ti.Type)
                            If generatedEntityType IsNot Nothing Then
                                typeName = generatedEntityType.FullName
                            End If
                        End If
                        If typeToExistsMap.ContainsKey(typeName) Then
                            Continue For
                        End If
                        If ti.IsPersistent Then
                            typeToExistsMap(typeName) = True
                            CreateSequece(uow, typeName)
                        End If
                    Next typeInfo
                    uow.CommitChanges()
                End Using
            End If
        End Sub
        'It may be necessary to use ValueManager if your application uses different databases for different users
        Public Shared ReadOnly Property DefaultDataLayer() As IDataLayer
            Get
                SyncLock syncRoot
                    If defaultDataLayer_Renamed Is Nothing Then
                        Dim disposableObjects() As IDisposable = Nothing
                        defaultDataLayer_Renamed = New SimpleDataLayer(XpoTypesInfoHelper.GetXpoTypeInfoSource().XPDictionary, DataStoreProvider.CreateUpdatingStore(False, disposableObjects))
                    End If
                    Return defaultDataLayer_Renamed
                End SyncLock
            End Get
        End Property
        Public Shared Sub Initialize(ByVal dataStoreProvider As IXpoDataStoreProvider)
            Guard.ArgumentNotNull(dataStoreProvider, "dataStoreProvider")
            SequenceGenerator.DataStoreProvider = dataStoreProvider
        End Sub
        Private Shared privateDataStoreProvider As IXpoDataStoreProvider
        Protected Shared Property DataStoreProvider() As IXpoDataStoreProvider
            Get
                Return privateDataStoreProvider
            End Get
            Private Set(ByVal value As IXpoDataStoreProvider)
                privateDataStoreProvider = value
            End Set
        End Property
    End Class
    'This persistent class is used to store last sequential number for persistent objects.
    Public Class Sequence
        Inherits XPBaseObject


        Private typeName_Renamed As String

        Private nextSequence_Renamed As Long
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        'Dennis: The size should be enough to store a full type name. However, you cannot use unlimited size for key columns.
        <Key, Size(1024)> _
        Public Property TypeName() As String
            Get
                Return typeName_Renamed
            End Get
            Set(ByVal value As String)
                SetPropertyValue("TypeName", typeName_Renamed, value)
            End Set
        End Property
        Public Property NextSequence() As Long
            Get
                Return nextSequence_Renamed
            End Get
            Set(ByVal value As Long)
                SetPropertyValue("NextSequence", nextSequence_Renamed, value)
            End Set
        End Property
    End Class
    Public Interface ISupportSequentialNumber
        Property SequentialNumber() As Long
    End Interface
End Namespace