<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128590685/15.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2829)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Contact.cs](./CS/Demo.Module/BusinessObjects/Contact.cs) (VB: [Contact.vb](./VB/Demo.Module/BusinessObjects/Contact.vb))
* [IDocument.cs](./CS/Demo.Module/BusinessObjects/IDocument.cs) (VB: [IDocument.vb](./VB/Demo.Module/BusinessObjects/IDocument.vb))
* [WebApplication.cs](./CS/Demo.Web/WebApplication.cs) (VB: [WebApplication.vb](./VB/Demo.Web/WebApplication.vb))
* [WinApplication.cs](./CS/Demo.Win/WinApplication.cs) (VB: [WinApplication.vb](./VB/Demo.Win/WinApplication.vb))
* [IUserFriendlyIdDomainComponent.cs](./CS/GenerateUserFriendlyId.Module/BusinessObjects/IUserFriendlyIdDomainComponent.cs)
* [SequenceGenerator.cs](./CS/GenerateUserFriendlyId.Module/BusinessObjects/SequenceGenerator.cs)
* [UserFriendlyIdPersistentObject.cs](./CS/GenerateUserFriendlyId.Module/BusinessObjects/UserFriendlyIdPersistentObject.cs)
* [Module.cs](./CS/GenerateUserFriendlyId.Module/Module.cs)
* [Updater.cs](./CS/GenerateUserFriendlyId.Module/Updater.cs) (VB: [Updater.vb](./VB/GenerateUserFriendlyId.Module/Updater.vb))
<!-- default file list end -->
# How to generate a sequential number for a persistent object within a database transaction (XAF)


<p>This example illustrates one of the possible approaches to implementing an identifier field with sequential values. Alternate approaches are listed in the <a href="https://www.devexpress.com/Support/Center/p/T567184">An overview of approaches to implementing a user-friendly sequential number for use with an XPO business class</a> KB article.<br><strong><br>Scenario</strong><br>This is a variation of the <a href="https://www.devexpress.com/Support/Center/p/E2620">How to generate and assign a sequential number for a business object within a database transaction, while being a part of a successful saving process</a> XPO example, which was specially adapted for XAF applications.</p>
<p>In particular, for better reusability and more smooth integration with the standard XAF CRUD Controllers, all the required operations to generate sequences are managed within the base persistent class automatically when a persistent object is being saved. For more developer convenience, this solution is organized as a reusable XAF module - <em>GenerateUserFriendlyId.Module.</em> This module consists of several key parts:</p>
<p><em>    - Sequence and</em> SequenceGenerator are auxiliary classes that take the main part in generating user-friendly identifiers. Take special note that the SequenceGenerator.Initialize method must be called during your XAF application startup for the correct operation.</p>
<p><em>    - UserFriendlyIdPersistentObject is a </em>base persistent class that subscribes to XPO's Session events and delegates calls to the core classes above. Normally, you must inherit your own business classes from this base class to get the described functionality in your project.</p>
<p><em>    - IUserFriendlyIdDomainComponent</em> is a base domain component that should be implemented by all domain components that require the described functionality.</p>
<p>Check the original example description first for more information on the demonstrated scenarios and functionality.</p>
<p><br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-generate-a-sequential-number-for-a-persistent-object-within-a-database-transaction-xaf-e2829/15.1.3+/media/9ecee31b-58bf-11e6-80bf-00155d62480c.png"></p>
<p><strong><br></strong><strong><br></strong></p>
<p><strong>Steps to implement</strong><br><strong>1.</strong> Copy and include the <em>GenerateUserFriendlyId.Module </em>project into your solution and make sure it is built successfully.  Invoke <a href="http://documentation.devexpress.com/#Xaf/CustomDocument2828">the Module or Application Designer</a> for the <em>YourSolutionName.Module/Module.xx </em> or <em>YourSolutionName.Wxx/WxxApplication.xx</em> files by double-clicking it in Solution Explorer. Invoke the Toolbox (Alt+X+T) and then drag & drop the <em>GenerateUserFriendlyIdModule</em> component into the modules list on the left.</p>
<p><strong>2.</strong> For apps with no security or with the Client-Side Security (XPObjectSpaceProvider or SecuredObjectSpaceProvider):<br>In the <em>YourSolutionName.Wxx/WxxApplication.xx</em> files, modify the <em>CreateDefaultObjectSpaceProvider </em>method to call the <em>SequenceGenerator.Initialize</em> method as shown in the <em>Demo.Wxx\WxxApplication.xx files</em>:</p>


```cs
protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
    IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true);
    // Web:
    // IXpoDataStoreProvider dataStoreProvider = GetDataStoreProvider(args.ConnectionString, args.Connection);
    GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(dataStoreProvider);
    ...
}

```


<p>For apps with the Middle Tier Security (DataServerObjectSpaceProvider):<br>In the <em>YourSolutionName.ApplicationServer project</em>, locate and modify the <em>serverApplication_CreateCustomObjectSpaceProvider</em> or<em> CreateDefaultObjectSpaceProvider  </em>methods to call the <em>SequenceGenerator.Initialize</em> method in the same manner.</p>
<p><strong><br>3.</strong> If you are using pure XPO classes, then inherit your required business classes from the module's <em>UserFriendlyIdPersistentObject</em> one and use the derived <em>SequenceNumber</em> property as required:</p>


```cs
    public class Contact : GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject {
        [PersistentAlias("concat('C', ToStr(SequentialNumber))")]
        public string ContactId {
            get {
                return Convert.ToString(EvaluateAlias("ContactId"));
            }
        }

```


<p>If you are using DC interfaces, then implement the <em>IUserFriendlyIdDomainComponent</em> interface by your custom domain component:</p>


```cs
    public interface IDocument : GenerateUserFriendlyId.Module.BusinessObjects.IUserFriendlyIdDomainComponent {
        [Calculated("concat('D', ToStr(SequentialNumber))")]
        string DocumentId { get; }

```


<p>Additionally for DC, use the <em>UserFriendlyIdPersistentObject</em> as a base class during your custom domain component registration, e.g.:</p>


```cs
XafTypesInfo.Instance.RegisterEntity("Document", typeof(IDocument), typeof(GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject));

```


<p>Note that the sequential number functionality shown in this example does not work with <a href="http://documentation.devexpress.com/#Xaf/DevExpressExpressAppDCITypesInfo_RegisterSharedParttopic"><u>DC shared parts</u></a> , because it requires a custom base class, which is not allowed for shared parts.<br><br><strong>4.</strong> For more information, download and review the <em>Address, Contact, and IDocument</em> types within the <em>Demo.Module</em> project. These are test business objects that demonstrate the use of the described functionality for XPO and DC respectively. A required format for the user-friendly identifier property in these end classes is defined within an aliased property (ContactId in the example above) by concatenation of a required constant or dynamic value with the <em>SequentialNumber</em> property provided by the base <em>UserFriendlyIdPersistentObject</em> class. So, if you need to have a different format, modify the PersistentAliasAttribute expression as your business needs dictate.</p>
<p> </p>
<p><strong>IMPORTANT NOTES</strong></p>
<p><strong>1.</strong> As an alternative, you can implement <strong>much simpler</strong> solutions at the database level or by using the built-in <strong><em>DistributedIdGeneratorHelper.Generate</em></strong> method. Refer to the <a href="https://www.devexpress.com/Support/Center/p/T567184">An overview of approaches to implementing a user-friendly sequential number for use with an XPO business class</a> article for more details.</p>
<p><strong>2.</strong>  In the <a href="https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument113436">Integrated Mode</a> and <a href="http://documentation.devexpress.com/#Xaf/CustomDocument3438"><u>middle-tier Application Server</u></a> scenario the newly generated sequence number will appear in the DetailView only after a manual refresh (i.e., it will be empty right away after saving a new record), because the sequence is generated on the server side only and is not passed to the client. You can <a href="https://www.devexpress.com/Support/Center/Attachment/GetAttachmentFile/Attachment/GetAttachment?fileOid=187e7170-8b1b-11e6-80bf-00155d62480c&fileName=E4904_E2829_MiddleTierApplicationServer(Console).zip">download a ready test project for these configurations here</a>. See also the "Refresh the Identifier field value in UI" section <a href="https://www.devexpress.com/Support/Center/p/T567184">in this KB article</a>.<br><strong>3.</strong> You can specify or seed the initial sequence value manually: either by editing the <strong>Sequence</strong> table in the database or using <a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113711.aspx">the standard XPO/XAF</a> means by manipulating the Sequence objects, e.g.:</p>


```cs
using(IObjectSpace os = Application.CreateObjectSpace(typeof(Sequence))) {
                    Sequence sequence = os.FindObject<Sequence>(CriteriaOperator.Parse("TypeName=?", typeof(E2829).FullName));
                    sequence.NextSequence = 5;
                    os.CommitChanges();
                }
```


<p><strong>See also:</strong><br><a href="https://www.devexpress.com/Support/Center/p/Q372524">How to create multiple sequences based on the a class property value, not one sequence per class type</a><br><br><br><strong>Your feedback is needed!<br></strong>We would greatly appreciate it if you share your feedback here in comments or rather <a href="https://www.devexpress.com/go/XAF_SequentialNumbers_T567184_Survey.aspx">participate in this short survey (~3 min)</a>.</p>

<br/>


