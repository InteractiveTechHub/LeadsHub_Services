using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Models;
using LeadsHub.Core.Enum;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeadsHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Company> Companies { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<ConsultantCompany> ConsultantCompanies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<WhatsAppConfig> WhatsAppConfigs { get; set; }
        public DbSet<WhatsAppTemplate> WhatsAppTemplates { get; set; }
        public DbSet<SalesPipeline> SalesPipelines { get; set; }
        public DbSet<PipelineStage> PipelineStages { get; set; }
        public DbSet<LeadStage> LeadStages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLead> ProductLeads { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<MessageText> MessageTexts { get; set; }
        public DbSet<MessageFile> MessageFiles { get; set; }
        public DbSet<MessageReaction> MessageReactions { get; set; }
        public DbSet<LastMessage> LastMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Company
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Identifier).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.BrandName).HasMaxLength(256).IsRequired();
                entity.Property(e => e.LegalName).HasMaxLength(256).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(400).IsRequired();
                entity.Property(e => e.IdentificationNumber).HasMaxLength(256).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
            });

            // Configure Consultant
            modelBuilder.Entity<Consultant>(entity =>
            {
                entity.ToTable("Consultant");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.IdentityId).IsRequired();
                entity.Property(e => e.FullName).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.NickName).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.PhotoUrl).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
            });

            // Configure ConsultantCompany
            modelBuilder.Entity<ConsultantCompany>(entity =>
            {
                entity.ToTable("ConsultantCompany");
                entity.HasKey(e => new { e.IdentityId, e.ConsultantId, e.CompanyId });
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.HasOne(e => e.Consultant)
                    .WithMany()
                    .HasForeignKey(e => e.ConsultantId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Contact
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.PhoneNumber).HasMaxLength(40).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.CPF).HasMaxLength(40).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
            });

            // Configure Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ZipCode).IsRequired();
                entity.Property(e => e.State).HasMaxLength(40).IsRequired();
                entity.Property(e => e.City).HasMaxLength(40).IsRequired();
                entity.Property(e => e.Street).HasMaxLength(400).IsRequired();
                entity.Property(e => e.Neighborhood).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Number).HasMaxLength(40);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Contact)
                    .WithMany()
                    .HasForeignKey(e => e.ContactId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Lead
            modelBuilder.Entity<Lead>(entity =>
            {
                entity.ToTable("Lead");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Identifier).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AdCode).HasMaxLength(100);
                entity.Property(e => e.Channel).HasColumnType("int");
                entity.Property(e => e.Phase).HasColumnType("smallint").HasDefaultValue(LeadPhase.New);
                entity.Property(e => e.Status).HasColumnType("smallint").HasDefaultValue(LeadStatus.Active);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(e => e.Contact)
                    .WithMany()
                    .HasForeignKey(e => e.ContactId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(e => e.Consultant)
                    .WithMany()
                    .HasForeignKey(e => e.ConsultantId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Integration
            modelBuilder.Entity<Integration>(entity =>
            {
                entity.ToTable("Integration");
                entity.HasKey(e => e.Id);
                
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.WhatsAppConfig)
                    .WithMany()
                    .HasForeignKey(e => e.WhatsAppConfigId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure WhatsAppConfig
            modelBuilder.Entity<WhatsAppConfig>(entity =>
            {
                entity.ToTable("WhatsAppConfig");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(600);
                entity.Property(e => e.PhoneNumberId).HasMaxLength(4000);
                entity.Property(e => e.BusinessAccountId).HasMaxLength(4000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure WhatsAppTemplate
            modelBuilder.Entity<WhatsAppTemplate>(entity =>
            {
                entity.ToTable("WhatsAppTemplate");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(400);
                entity.Property(e => e.Language).HasMaxLength(10);
                entity.Property(e => e.Type).HasColumnType("smallint");
                entity.Property(e => e.Variables).HasMaxLength(1000);
                entity.Property(e => e.Category).HasMaxLength(255);
                entity.Property(e => e.Status).HasMaxLength(100);
                entity.Property(e => e.Enabled).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.WhatsAppConfig)
                    .WithMany(w => w.WhatsAppTemplates)
                    .HasForeignKey(e => e.WhatsAppConfigId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SalesPipeline
            modelBuilder.Entity<SalesPipeline>(entity =>
            {
                entity.ToTable("SalesPipeline");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(1000);
                entity.Property(e => e.Position).HasColumnType("smallint").IsRequired();
                
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Consultant)
                    .WithMany()
                    .HasForeignKey(e => e.ConsultantId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure PipelineStage
            modelBuilder.Entity<PipelineStage>(entity =>
            {
                entity.ToTable("PipelineStage");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(1000).IsRequired();
                entity.Property(e => e.Position).HasColumnType("smallint").IsRequired();
                
                entity.HasOne(e => e.SalesPipeline)
                    .WithMany(s => s.Stages)
                    .HasForeignKey(e => e.SalesPipelineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LeadStage
            modelBuilder.Entity<LeadStage>(entity =>
            {
                entity.ToTable("LeadStage");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Position).HasColumnType("smallint").IsRequired();
                
                entity.Ignore(e => e.LeadCard);
                
                entity.HasOne(e => e.Lead)
                    .WithMany()
                    .HasForeignKey(e => e.LeadId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.PipelineStage)
                    .WithMany(p => p.Leads)
                    .HasForeignKey(e => e.PipelineStageId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Category).HasMaxLength(250);
                entity.Property(e => e.ProductCode).HasMaxLength(250);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
            });

            // Configure ProductLead
            modelBuilder.Entity<ProductLead>(entity =>
            {
                entity.ToTable("ProductLead");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.RelationshipDate).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Lead)
                    .WithMany()
                    .HasForeignKey(e => e.LeadId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Timeline
            modelBuilder.Entity<Timeline>(entity =>
            {
                entity.ToTable("Timeline");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MessageId).HasMaxLength(100);
                entity.Property(e => e.MessageDate).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.Sender).HasColumnType("smallint").HasDefaultValue(MessageSender.consultant);
                entity.Property(e => e.Type).HasColumnType("smallint").HasDefaultValue(MessageType.Text);
                entity.Property(e => e.Status).HasColumnType("smallint").HasDefaultValue(MessageStatus.Sent);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                
                entity.HasOne(e => e.Lead)
                    .WithMany(l => l.Timelines)
                    .HasForeignKey(e => e.LeadId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Consultant)
                    .WithMany()
                    .HasForeignKey(e => e.ConsultantId)
                    .OnDelete(DeleteBehavior.NoAction);
                    
                entity.HasOne(e => e.Message)
                    .WithMany()
                    .HasForeignKey(e => e.MessageTextId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.MessageFile)
                    .WithMany()
                    .HasForeignKey(e => e.MessageFileId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.MessageReaction)
                    .WithMany()
                    .HasForeignKey(e => e.MessageReactionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure MessageText
            modelBuilder.Entity<MessageText>(entity =>
            {
                entity.ToTable("MessageText");
                entity.HasKey(e => e.Id);
            });

            // Configure MessageFile
            modelBuilder.Entity<MessageFile>(entity =>
            {
                entity.ToTable("MessageFile");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MimeType).HasMaxLength(100);
                entity.Property(e => e.Caption).HasMaxLength(1024);
            });

            // Configure MessageReaction
            modelBuilder.Entity<MessageReaction>(entity =>
            {
                entity.ToTable("MessageReaction");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Emoji).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("timezone('UTC', CURRENT_TIMESTAMP)");
            });

            // Configure LastMessage
            modelBuilder.Entity<LastMessage>(entity =>
            {
                entity.ToTable("LastMessage");
                entity.HasKey(e => e.LeadId);
                entity.Property(e => e.Status).HasColumnType("smallint").HasDefaultValue(1);
                
                entity.HasOne(e => e.Lead)
                    .WithOne()
                    .HasForeignKey<LastMessage>(e => e.LeadId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(e => e.Timeline)
                    .WithMany()
                    .HasForeignKey(e => e.TimelineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LeadCard (keyless entity)
            modelBuilder.Entity<LeadCard>(entity =>
            {
                entity.ToTable("LeadCard");
                entity.HasNoKey();
            });

            // Configure unaccent extension
            modelBuilder.HasPostgresExtension("unaccent");
        }
    }
}
