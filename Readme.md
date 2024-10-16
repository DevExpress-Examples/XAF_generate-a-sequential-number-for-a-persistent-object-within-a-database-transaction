<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128590685/23.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2829)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# XAF - How to generate a sequential number for a persistent object within a database transaction with Entity Framework Core

This example illustrates how to implement a business object with an identifier field with autogenerated sequential values.

![image](https://github.com/AndreyKozhevnikov/XAF_generate-a-sequential-number-for-a-persistent-object-within-a-database-transaction/assets/14300209/2883e55f-23ec-488e-ad7e-1410968160c0)


This Readme focuses on Entity Framework Core. For information on how to achieve the same functionality with XPO, see the [Readme.md](CS/XPO/Readme.md) file in the XPO solution's folder.

## Implementation Details



Entity Framework Core allows you to set up generation of sequential values for non-key data fields as described in the following article in the Entity Framework Core documentation: [Generated Values - Explicitly configuring value generation](https://learn.microsoft.com/en-us/ef/core/modeling/generated-properties?tabs=data-annotations#explicitly-configuring-value-generation).

Use the steps below to generate sequential values for a business object's property so that the property is assigned a value once the object has been saved to the database:

1. Add the `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` attribute to the required business object property:

   ```cs
   public class Address : BaseObject {
       // ...
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public virtual long SequentialNumber { get; set; }
       // ...
   }
   ```

2. Add the following code to the DbContext's `OnModelCreating` method implementation so that the generated values are always displayed in the UI immediately after the object has been saved:

   ```cs
   public class GenerateUserFriendlyIdEFCoreDbContext : DbContext {
       // ...
       protected override void OnModelCreating(ModelBuilder modelBuilder) {
           // ...
           modelBuilder.Entity<Address>().UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
       }
   }
   ```

As an alternative technique, you can use a database-specific _sequence_ object to generate sequential values as described in the following article: [Sequences](https://learn.microsoft.com/en-us/ef/core/modeling/sequences).

## Files to Review

- [Address.cs](./CS/EFCore/GenerateUserFriendlyId.Module/BusinessObjects/Address.cs)
- [Contact.cs](./CS/EFCore/GenerateUserFriendlyId.Module/BusinessObjects/Contact.cs)
- [Document.cs](./CS/EFCore/GenerateUserFriendlyId.Module/BusinessObjects/Document.cs)
- [GenerateUserFriendlyIdDbContext.cs](./CS/EFCore/GenerateUserFriendlyId.Module/BusinessObjects/GenerateUserFriendlyIdDbContext.cs)

## Documentation

* [Business Model Design with Entity Framework Core](https://docs.devexpress.com/eXpressAppFramework/401886/business-model-design-orm/business-model-design-with-entity-framework-core)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=XAF_generate-a-sequential-number-for-a-persistent-object-within-a-database-transaction&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=XAF_generate-a-sequential-number-for-a-persistent-object-within-a-database-transaction&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
