using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using Demo.Module.BusinessObjects;

namespace GenerateUserFriendlyId.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class GenerateUserFriendlyIdContextInitializer : DbContextTypesInfoInitializerBase {
	protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<GenerateUserFriendlyIdEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new GenerateUserFriendlyIdEFCoreDbContext(optionsBuilder.Options);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class GenerateUserFriendlyIdDesignTimeDbContextFactory : IDesignTimeDbContextFactory<GenerateUserFriendlyIdEFCoreDbContext> {
	public GenerateUserFriendlyIdEFCoreDbContext CreateDbContext(string[] args) {
		throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        //var optionsBuilder = new DbContextOptionsBuilder<GenerateUserFriendlyIdEFCoreDbContext>();
        //optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=E2829EFCore");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
        //return new GenerateUserFriendlyIdEFCoreDbContext(optionsBuilder.Options);
    }
}
[TypesInfoInitializer(typeof(GenerateUserFriendlyIdContextInitializer))]
public class GenerateUserFriendlyIdEFCoreDbContext : DbContext {
	public GenerateUserFriendlyIdEFCoreDbContext(DbContextOptions<GenerateUserFriendlyIdEFCoreDbContext> options) : base(options) {
	}
	
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.Entity<Address>().UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
        modelBuilder.Entity<Contact>().UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
        modelBuilder.Entity<Document>().UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
    }
}
