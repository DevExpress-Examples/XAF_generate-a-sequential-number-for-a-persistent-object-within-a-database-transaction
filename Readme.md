<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128590685/22.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2829)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to generate a sequential number for a persistent object within a database transaction

> **Note**  
> The description of this example is currently under construction and may not match the code in the example. We are currently working to provide you with up-to-date content.

This example illustrates one of the possible approaches to implementing an identifier field with sequential values.

![](https://raw.githubusercontent.com/DevExpress-Examples/how-to-generate-a-sequential-number-for-a-persistent-object-within-a-database-transaction-xaf-e2829/17.2.8+/media/9ecee31b-58bf-11e6-80bf-00155d62480c.png)

## Scenario

This is a variation of the [How to generate and assign a sequential number for a business object within a database transaction, while being a part of a successful saving process](https://www.devexpress.com/Support/Center/p/E2620) XPO example, which was specially adapted for XAF applications.

In particular, for better reusability and more smooth integration with the standard XAF CRUD Controllers, all the required operations to generate sequences are managed within the base persistent class automatically when a persistent object is being saved. For more developer convenience, this solution is organized as a reusable XAF module (_GenerateUserFriendlyId.Module_). This module consists of several key parts:

* `Sequence` and `SequenceGenerator` are auxiliary classes that take the main part in generating user-friendly identifiers. Take special note that the `SequenceGenerator.Initialize` method must be called during your XAF application startup for the correct operation.
* `UserFriendlyIdPersistentObject` is a base persistent class that subscribes to XPO's Session events and delegates calls to the core classes above. Normally, you must inherit your own business classes from this base class to get the described functionality in your project.
* `IUserFriendlyIdDomainComponent` is a base domain component that should be implemented by all domain components that require the described functionality.

Check the original example description first for more information on the demonstrated scenarios and functionality.

## Implementation Details

1. Copy and include the _GenerateUserFriendlyId.Module_ project into your solution and make sure it is built successfully.  Invoke the [Module or Application Designer](https://docs.devexpress.com/eXpressAppFramework/112828/installation-upgrade-version-history/visual-studio-integration/module-designer) for the _YourSolutionName.Module/Module.xx_ or _YourSolutionName.Wxx/WxxApplication.xx_ files by double-clicking it in Solution Explorer. Invoke the Toolbox (Alt+X+T) and then drag & drop the _GenerateUserFriendlyIdModule_ component into the modules list on the left.

2. For apps with no security or with the Client-Side Security (`XPObjectSpaceProvider` or `SecuredObjectSpaceProvider`):
   
   In the _YourSolutionName.Wxx/WxxApplication.xx_ files, modify the `CreateDefaultObjectSpaceProvider` method to call the `SequenceGenerator.Initialize` method as shown in the _Demo.Wxx\WxxApplication.xx_ files:
   
   ```cs
   protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
       IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(args.ConnectionString, args.Connection, true);
       // Web:
       // IXpoDataStoreProvider dataStoreProvider = GetDataStoreProvider(args.ConnectionString, args.Connection);
       GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(dataStoreProvider);
       ...
   }
   
   ```
   
   For apps with the Middle Tier Security (`DataServerObjectSpaceProvider`):
   
   In the _YourSolutionName.ApplicationServer_ project, locate and modify the `serverApplication_CreateCustomObjectSpaceProvider` or `CreateDefaultObjectSpaceProvider`  methods to call the `SequenceGenerator.Initialize` method in the same manner.

3. If you are using pure XPO classes, then inherit your business classes to which you want to add sequential numbers from the module's `UserFriendlyIdPersistentObject` class. Declare a calculated property that uses the `SequenceNumber` property of the base class to produce an string identifier according to the required format:
   
   ```cs
   public class Contact : GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject {
       [PersistentAlias("Concat('C',PadLeft(ToStr(SequentialNumber),6,'0'))")]
       public string ContactId {
           get { return Convert.ToString(EvaluateAlias("ContactId")); }
       }
   
   ```
   
   If you are using DC interfaces, then implement the `IUserFriendlyIdDomainComponent` interface by your custom domain component:
   
   ```cs
   public interface IDocument : GenerateUserFriendlyId.Module.BusinessObjects.IUserFriendlyIdDomainComponent {
       [Calculated("Concat('D',PadLeft(ToStr(SequentialNumber),6,'0'))")]
       string DocumentId { get; }
   
   ```
   
   Additionally for DC, use `UserFriendlyIdPersistentObject` as a base class during your custom domain component registration, for example:
   
   ```cs
   XafTypesInfo.Instance.RegisterEntity("Document", typeof(IDocument), typeof(GenerateUserFriendlyId.Module.BusinessObjects.UserFriendlyIdPersistentObject));
   
   ```
   
   Note, that the sequential number functionality shown in this example does not work with [DC shared parts](http://documentation.devexpress.com/#Xaf/DevExpressExpressAppDCITypesInfo_RegisterSharedParttopic), because it requires a custom base class, which is not allowed for shared parts.
   
4. By default, separate sequences are generated for each business object type. If you need to create multiple sequences for the same type, based on values of other object properties, override the `GetSequenceName` method and return the costructed sequence name. The `Address` class in this example uses separate sequences for each `Province` as follows:
   
   ```cs
   protected override string GetSequenceName() {
       return string.Concat(ClassInfo.FullName, "-", Province.Replace(" ", "_"));
   }
   ```
   
5. For more information, download and review the `Address`, `Contact`, and `IDocument` types within the _Demo.Module_ project. These are test business objects that demonstrate the use of the described functionality for XPO and DC respectively. Feel free to modify this example to add functionality as your business needs dictate.

## Additional Information

1. As an alternative, you can implement much simpler solutions at the database level or by using the built-in `DistributedIdGeneratorHelper.Generate` method. Refer to the following article for more details: [An overview of approaches to implementing a user-friendly sequential number for use with an XPO business class](https://www.devexpress.com/Support/Center/p/T567184").

2. In the [Integrated Mode](https://docs.devexpress.com/eXpressAppFramework/113436/data-security-and-safety/security-system/security-tiers/2-tier-security-integrated-mode-and-ui-level) and [Middle Tier Application Server](https://docs.devexpress.com/eXpressAppFramework/113439/data-security-and-safety/security-system/security-tiers/middle-tier-security) scenario, the newly generated sequence number will appear in the Detail View only after a manual refresh (i.e., it will be empty right away after saving a new record), because the sequence is generated on the server side only and is not passed to the client. See the following section of the KB article: [Refresh the Identifier field value in UI](https://docs.devexpress.com/eXpressAppFramework/403605/business-model-design-orm/unique-auto-increment-number-generation#refresh-the-identifier-field-value-in-the-ui).

3. You can specify or seed the initial sequence value manually: either by editing the **Sequence** table in the database or using the [standard XPO/XAF](https://docs.devexpress.com/eXpressAppFramework/113711/data-manipulation-and-business-logic/create-read-update-and-delete-data) means by manipulating the `Sequence` objects, for example:

   ```cs
   using(IObjectSpace os = Application.CreateObjectSpace(typeof(Sequence))) {
       Sequence sequence = os.FindObject<Sequence>(CriteriaOperator.Parse("TypeName=?", typeof(Contact).FullName));
       sequence.NextSequence = 5;
       os.CommitChanges();
   }
   ```
   
## Documentation
   
* [An overview of approaches to implementing a user-friendly sequential number for use with an XPO business class](https://www.devexpress.com/Support/Center/p/T567184)

## Examples

* [XAF XPO - How to Generate a Sequential User-friendly Identifier Field](https://github.com/DevExpress-Examples/XAF_how-to-generate-a-sequential-and-user-friendly-identifier-field-within-an-xpo-business-e4904)
