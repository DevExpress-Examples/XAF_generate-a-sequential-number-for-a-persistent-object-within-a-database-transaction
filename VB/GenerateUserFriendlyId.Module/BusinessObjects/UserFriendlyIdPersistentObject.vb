Imports System
Imports DevExpress.Xpo
Imports System.ComponentModel
Imports System.Collections.Generic
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports GenerateUserFriendlyId.Module

Namespace GenerateUserFriendlyId.Module.BusinessObjects
    'Dennis: Uncomment this code if you want to have the SequentialNumber column created in each derived class table.
    <NonPersistent> _
    Public MustInherit Class UserFriendlyIdPersistentObject
        Inherits BaseObject
        Implements ISupportSequentialNumber

        Private _SequentialNumber As Long
        Private Shared sequenceGenerator As SequenceGenerator
        Private Shared syncRoot As New Object()
        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        <Browsable(False), Indexed(Unique := False)> _
        Public Property SequentialNumber() As Long Implements ISupportSequentialNumber.SequentialNumber
        'Dennis: Comment out this code if you do not want to have the SequentialNumber column created in each derived class table.
            Get
                Return _SequentialNumber
            End Get
            Set(ByVal value As Long)
                SetPropertyValue("SequentialNumber", _SequentialNumber, value)
            End Set
        End Property
        Private Sub OnSequenceGenerated(ByVal newId As Long)
            SequentialNumber = newId
        End Sub
        Protected Overrides Sub OnSaving()
            Try
                MyBase.OnSaving()
                If Not(TypeOf Session Is NestedUnitOfWork) AndAlso (Session.DataLayer IsNot Nothing) AndAlso (TypeOf Session.ObjectLayer Is SimpleObjectLayer) AndAlso Session.IsNewObject(Me) Then
                        'OR
                        '&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                    GenerateSequence()
                End If
            Catch
                CancelSequence()
                Throw
            End Try
        End Sub
        Public Sub GenerateSequence()
            SyncLock syncRoot
                Dim typeToExistsMap As New Dictionary(Of String, Boolean)()
                For Each item As Object In Session.GetObjectsToSave()
                    typeToExistsMap(Session.GetClassInfo(item).FullName) = True
                Next item
                If sequenceGenerator Is Nothing Then
                    sequenceGenerator = New SequenceGenerator(typeToExistsMap)
                End If
                SubscribeToEvents()
                OnSequenceGenerated(sequenceGenerator.GetNextSequence(ClassInfo))
            End SyncLock
        End Sub
        Private Sub AcceptSequence()
            SyncLock syncRoot
                If sequenceGenerator IsNot Nothing Then
                    Try
                        sequenceGenerator.Accept()
                    Finally
                        CancelSequence()
                    End Try
                End If
            End SyncLock
        End Sub
        Private Sub CancelSequence()
            SyncLock syncRoot
                UnSubscribeFromEvents()
                If sequenceGenerator IsNot Nothing Then
                    sequenceGenerator.Close()
                    sequenceGenerator = Nothing
                End If
            End SyncLock
        End Sub
        Private Sub Session_AfterCommitTransaction(ByVal sender As Object, ByVal e As SessionManipulationEventArgs)
            AcceptSequence()
        End Sub
        Private Sub Session_AfterRollBack(ByVal sender As Object, ByVal e As SessionManipulationEventArgs)
            CancelSequence()
        End Sub
        Private Sub Session_FailedCommitTransaction(ByVal sender As Object, ByVal e As SessionOperationFailEventArgs)
            CancelSequence()
        End Sub
        Private Sub SubscribeToEvents()
            If Not(TypeOf Session Is NestedUnitOfWork) Then
                AddHandler Session.AfterCommitTransaction, AddressOf Session_AfterCommitTransaction
                AddHandler Session.AfterRollbackTransaction, AddressOf Session_AfterRollBack
                AddHandler Session.FailedCommitTransaction, AddressOf Session_FailedCommitTransaction
            End If
        End Sub
        Private Sub UnSubscribeFromEvents()
            If Not(TypeOf Session Is NestedUnitOfWork) Then
                RemoveHandler Session.AfterCommitTransaction, AddressOf Session_AfterCommitTransaction
                RemoveHandler Session.AfterRollbackTransaction, AddressOf Session_AfterRollBack
                RemoveHandler Session.FailedCommitTransaction, AddressOf Session_FailedCommitTransaction
            End If
        End Sub
    End Class
End Namespace