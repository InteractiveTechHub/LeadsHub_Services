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

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
                new { Id = 1L, BrandName = "TechCorp Solutions", LegalName = "TechCorp Solutions Ltda", Email = "contato@techcorp.com.br", IdentificationNumber = "12.345.678/0001-90", PhoneNumber = "(11) 99999-9999", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, BrandName = "Inovação Digital", LegalName = "Inovação Digital S.A.", Email = "vendas@inovacaodigital.com.br", IdentificationNumber = "98.765.432/0001-10", PhoneNumber = "(21) 88888-8888", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, BrandName = "StartupHub", LegalName = "StartupHub Tecnologia Ltda", Email = "info@startuphub.com.br", IdentificationNumber = "11.222.333/0001-44", PhoneNumber = "(31) 77777-7777", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Addresses
            modelBuilder.Entity<Address>().HasData(
                new { Id = 1L, CompanyId = 1L, ZipCode = "01234-567", State = "SP", City = "São Paulo", Street = "Av. Paulista", Neighborhood = "Bela Vista", Number = "1000", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, CompanyId = 2L, ZipCode = "20000-000", State = "RJ", City = "Rio de Janeiro", Street = "Rua da Carioca", Neighborhood = "Centro", Number = "500", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, CompanyId = 3L, ZipCode = "30000-000", State = "MG", City = "Belo Horizonte", Street = "Av. Afonso Pena", Neighborhood = "Centro", Number = "2000", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new { Id = 1L, Name = "Sistema CRM Completo", Description = "Sistema completo de gestão de relacionamento com clientes", Category = "Software", ProductCode = "CRM-001", Price = 299.90m, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, Name = "Plataforma de E-commerce", Description = "Solução completa para loja virtual", Category = "Software", ProductCode = "ECO-001", Price = 499.90m, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, Name = "Consultoria em Transformação Digital", Description = "Consultoria especializada em transformação digital para empresas", Category = "Consultoria", ProductCode = "CON-001", Price = 150.00m, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 4L, Name = "Desenvolvimento de App Mobile", Description = "Desenvolvimento de aplicativo mobile personalizado", Category = "Desenvolvimento", ProductCode = "APP-001", Price = 2500.00m, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, Name = "Hospedagem Cloud Premium", Description = "Hospedagem em nuvem com alta disponibilidade", Category = "Infraestrutura", ProductCode = "HOS-001", Price = 99.90m, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Contacts
            modelBuilder.Entity<Contact>().HasData(
                new { Id = 1L, Name = "Carlos Eduardo Mendes", Email = "carlos.mendes@email.com", PhoneNumber = "(11) 98765-4321", CPF = "123.456.789-00", BirthDate = new DateTime(1985, 3, 15), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, Name = "Ana Paula Ferreira", Email = "ana.ferreira@email.com", PhoneNumber = "(21) 97654-3210", CPF = "234.567.890-11", BirthDate = new DateTime(1990, 7, 22), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, Name = "Roberto Silva Lima", Email = "roberto.lima@email.com", PhoneNumber = "(31) 96543-2109", CPF = "345.678.901-22", BirthDate = new DateTime(1988, 11, 8), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 4L, Name = "Fernanda Costa Santos", Email = "fernanda.santos@email.com", PhoneNumber = "(11) 95432-1098", CPF = "456.789.012-33", BirthDate = new DateTime(1992, 5, 30), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, Name = "Pedro Henrique Oliveira", Email = "pedro.oliveira@email.com", PhoneNumber = "(21) 94321-0987", CPF = "567.890.123-44", BirthDate = new DateTime(1987, 9, 12), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 6L, Name = "Juliana Rodrigues Alves", Email = "juliana.alves@email.com", PhoneNumber = "(31) 93210-9876", CPF = "678.901.234-55", BirthDate = new DateTime(1993, 1, 25), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed WhatsApp Configs
            modelBuilder.Entity<WhatsAppConfig>().HasData(
                new { Id = 1L, CompanyId = 1L, Name = "WhatsApp - TechCorp Solutions", AccessToken = "token_1_" + "static-guid-123", BusinessAccountId = "business_1", PhoneNumberId = "phone_1", WebHookSecret = "secret_1_" + "static-guid-123", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, CompanyId = 2L, Name = "WhatsApp - Inovação Digital", AccessToken = "token_2_" + "static-guid-123", BusinessAccountId = "business_2", PhoneNumberId = "phone_2", WebHookSecret = "secret_2_" + "static-guid-123", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, CompanyId = 3L, Name = "WhatsApp - StartupHub", AccessToken = "token_3_" + "static-guid-123", BusinessAccountId = "business_3", PhoneNumberId = "phone_3", WebHookSecret = "secret_3_" + "static-guid-123", Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Integrations
            modelBuilder.Entity<Integration>().HasData(
                new { Id = 1L, CompanyId = 1L, WhatsAppConfigId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, CompanyId = 2L, WhatsAppConfigId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, CompanyId = 3L, WhatsAppConfigId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );


            // Seed MessageTexts
            modelBuilder.Entity<MessageText>().HasData(
                new { Id = 1L, Body = "Olá! Vi seu anúncio sobre o sistema CRM. Gostaria de saber mais informações sobre os preços e funcionalidades.", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, Body = "Olá Carlos! Obrigado pelo interesse. Nosso sistema CRM completo custa R$ 299,90/mês e inclui gestão de leads, pipeline de vendas, relatórios e integração com WhatsApp. Posso agendar uma demonstração?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, Body = "Perfeito! Qual seria o melhor horário para a demonstração? Preciso de algo que funcione bem para minha equipe de 5 pessoas.", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 4L, Body = "Oi! Vi que vocês desenvolvem e-commerce. Preciso de uma loja virtual para minha empresa de roupas. Vocês fazem integração com pagamentos?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, Body = "Olá Ana! Sim, desenvolvemos e-commerces completos com integração a todos os principais gateways de pagamento (PagSeguro, Mercado Pago, PayPal). Nossa plataforma custa R$ 499,90/mês. Posso enviar alguns cases de sucesso?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 6L, Body = "Sim, por favor! E qual seria o prazo para desenvolvimento? Preciso lançar em 2 meses.", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 7L, Body = "Olá! Gostaria de agendar uma reunião para falar sobre transformação digital na minha empresa. Vocês fazem consultoria?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 8L, Body = "Olá Roberto! Sim, temos consultoria especializada em transformação digital. Nossa consultoria custa R$ 150/hora e inclui análise completa da empresa. Que dia seria melhor para a reunião?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 9L, Body = "Perfeito! Que tal quinta-feira às 14h? Preciso de ajuda com automação de processos e migração para nuvem.", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 10L, Body = "Excelente! Confirmado para quinta às 14h. Vou preparar uma apresentação com nosso plano de transformação digital. Até lá!", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 11L, Body = "Oi! Preciso de um app mobile para minha startup. Vocês desenvolvem apps nativos ou híbridos?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 12L, Body = "Olá Pedro! Desenvolvemos tanto apps nativos quanto híbridos. Para startups, recomendamos React Native (híbrido) que custa R$ 2.500 e tem prazo de 6-8 semanas. Qual tipo de app você precisa?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 13L, Body = "Preciso de um app de delivery. Vocês já fizeram algo similar? Tem referências?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 14L, Body = "Sim! Já desenvolvemos 3 apps de delivery. Posso enviar os cases e agendar uma call para detalhar o projeto?", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 15L, Body = "Olá! Vi o anúncio do Facebook sobre desenvolvimento de apps. Mas já fechamos com outra empresa. Obrigado mesmo assim!", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 16L, Body = "Sem problemas, Juliana! Se precisar de algo no futuro, estaremos aqui. Boa sorte com o projeto!", CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed ApplicationUsers
            modelBuilder.Entity<ApplicationUser>().HasData(
                new { Id = "user-001", UserName = "joao.silva@techcorp.com.br", NormalizedUserName = "JOAO.SILVA@TECHCORP.COM.BR", Email = "joao.silva@techcorp.com.br", NormalizedEmail = "JOAO.SILVA@TECHCORP.COM.BR", EmailConfirmed = true, PhoneNumber = "(11) 99999-9999", PhoneNumberConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEHashExample123", SecurityStamp = "SecurityStamp123", ConcurrencyStamp = "ConcurrencyStamp123", LockoutEnabled = true, AccessFailedCount = 0, TwoFactorEnabled = false, Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = "user-002", UserName = "maria.santos@inovacaodigital.com.br", NormalizedUserName = "MARIA.SANTOS@INOVACAODIGITAL.COM.BR", Email = "maria.santos@inovacaodigital.com.br", NormalizedEmail = "MARIA.SANTOS@INOVACAODIGITAL.COM.BR", EmailConfirmed = true, PhoneNumber = "(21) 88888-8888", PhoneNumberConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEHashExample123", SecurityStamp = "SecurityStamp456", ConcurrencyStamp = "ConcurrencyStamp456", LockoutEnabled = true, AccessFailedCount = 0, TwoFactorEnabled = false, Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = "user-003", UserName = "pedro.oliveira@startuphub.com.br", NormalizedUserName = "PEDRO.OLIVEIRA@STARTUPHUB.COM.BR", Email = "pedro.oliveira@startuphub.com.br", NormalizedEmail = "PEDRO.OLIVEIRA@STARTUPHUB.COM.BR", EmailConfirmed = true, PhoneNumber = "(31) 77777-7777", PhoneNumberConfirmed = true, PasswordHash = "AQAAAAEAACcQAAAAEHashExample123", SecurityStamp = "SecurityStamp789", ConcurrencyStamp = "ConcurrencyStamp789", LockoutEnabled = true, AccessFailedCount = 0, TwoFactorEnabled = false, Enabled = true, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Consultants
            modelBuilder.Entity<Consultant>().HasData(
                new { Id = 1L, IdentityId = "user-001", Enabled = true, FullName = "João Silva", NickName = "João", PhotoUrl = "https://example.com/joao.jpg", TimeLastLeadAssigned = (DateTimeOffset?)null, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, IdentityId = "user-002", Enabled = true, FullName = "Maria Santos", NickName = "Maria", PhotoUrl = "https://example.com/maria.jpg", TimeLastLeadAssigned = (DateTimeOffset?)null, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, IdentityId = "user-003", Enabled = true, FullName = "Pedro Oliveira", NickName = "Pedro", PhotoUrl = "https://example.com/pedro.jpg", TimeLastLeadAssigned = (DateTimeOffset?)null, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed ConsultantCompanies (relacionamento entre consultores e empresas)
            modelBuilder.Entity<ConsultantCompany>().HasData(
                new { Id = 1L, IdentityId = "user-001", ConsultantId = 1L, CompanyId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, IdentityId = "user-002", ConsultantId = 2L, CompanyId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, IdentityId = "user-003", ConsultantId = 3L, CompanyId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Atualizar Leads para incluir ConsultantId
            modelBuilder.Entity<Lead>().HasData(
                new { Id = 1L, CompanyId = 1L, ContactId = 1L, IntegrationId = 1L, Channel = 1, Phase = LeadPhase.New, Status = LeadStatus.Active, AdCode = "AD001", SaleNote = "Cliente interessado em CRM, aguardando proposta", ConsultantId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, CompanyId = 1L, ContactId = 2L, IntegrationId = 1L, Channel = 2, Phase = LeadPhase.InProgress, Status = LeadStatus.Active, AdCode = "FB001", SaleNote = "Cliente em processo de negociação para e-commerce", ConsultantId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, CompanyId = 2L, ContactId = 3L, IntegrationId = 2L, Channel = 1, Phase = LeadPhase.Appointment, Status = LeadStatus.Active, AdCode = "AD002", SaleNote = "Reunião agendada para apresentação da solução", ConsultantId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 4L, CompanyId = 2L, ContactId = 4L, IntegrationId = 2L, Channel = 3, Phase = LeadPhase.Closed, Status = LeadStatus.ClosedWon, AdCode = "GOOGLE001", SaleNote = "Venda concluída - Consultoria em Transformação Digital", ConsultantId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, CompanyId = 3L, ContactId = 5L, IntegrationId = 3L, Channel = 1, Phase = LeadPhase.InProgress, Status = LeadStatus.Active, AdCode = "AD003", SaleNote = "Cliente interessado em desenvolvimento de app", ConsultantId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 6L, CompanyId = 3L, ContactId = 6L, IntegrationId = 3L, Channel = 2, Phase = LeadPhase.Closed, Status = LeadStatus.ClosedLost, AdCode = "FB002", SaleNote = "Cliente optou por concorrente", ConsultantId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed Timelines (conversas) - agora com ConsultantId correto
            modelBuilder.Entity<Timeline>().HasData(
                // Lead 1 - Carlos (CRM) - João Silva
                new { Id = 1L, LeadId = 1L, ConsultantId = 1L, MessageId = "msg_001", MessageDate = new DateTimeOffset(2024, 1, 1, 10, 0, 0, TimeSpan.Zero), MessageTextId = 1L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, LeadId = 1L, ConsultantId = 1L, MessageId = "msg_002", MessageDate = new DateTimeOffset(2024, 1, 1, 10, 5, 0, TimeSpan.Zero), MessageTextId = 2L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, LeadId = 1L, ConsultantId = 1L, MessageId = "msg_003", MessageDate = new DateTimeOffset(2024, 1, 1, 10, 10, 0, TimeSpan.Zero), MessageTextId = 3L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 2 - Ana (E-commerce) - João Silva
                new { Id = 4L, LeadId = 2L, ConsultantId = 1L, MessageId = "msg_004", MessageDate = new DateTimeOffset(2024, 1, 1, 11, 0, 0, TimeSpan.Zero), MessageTextId = 4L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, LeadId = 2L, ConsultantId = 1L, MessageId = "msg_005", MessageDate = new DateTimeOffset(2024, 1, 1, 11, 5, 0, TimeSpan.Zero), MessageTextId = 5L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 6L, LeadId = 2L, ConsultantId = 1L, MessageId = "msg_006", MessageDate = new DateTimeOffset(2024, 1, 1, 11, 10, 0, TimeSpan.Zero), MessageTextId = 6L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 3 - Roberto (Consultoria) - Maria Santos
                new { Id = 7L, LeadId = 3L, ConsultantId = 2L, MessageId = "msg_007", MessageDate = new DateTimeOffset(2024, 1, 1, 14, 0, 0, TimeSpan.Zero), MessageTextId = 7L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 8L, LeadId = 3L, ConsultantId = 2L, MessageId = "msg_008", MessageDate = new DateTimeOffset(2024, 1, 1, 14, 5, 0, TimeSpan.Zero), MessageTextId = 8L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 9L, LeadId = 3L, ConsultantId = 2L, MessageId = "msg_009", MessageDate = new DateTimeOffset(2024, 1, 1, 14, 10, 0, TimeSpan.Zero), MessageTextId = 9L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 10L, LeadId = 3L, ConsultantId = 2L, MessageId = "msg_010", MessageDate = new DateTimeOffset(2024, 1, 1, 14, 15, 0, TimeSpan.Zero), MessageTextId = 10L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 5 - Pedro (App Mobile) - Pedro Oliveira
                new { Id = 11L, LeadId = 5L, ConsultantId = 3L, MessageId = "msg_011", MessageDate = new DateTimeOffset(2024, 1, 1, 15, 0, 0, TimeSpan.Zero), MessageTextId = 11L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 12L, LeadId = 5L, ConsultantId = 3L, MessageId = "msg_012", MessageDate = new DateTimeOffset(2024, 1, 1, 15, 5, 0, TimeSpan.Zero), MessageTextId = 12L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 13L, LeadId = 5L, ConsultantId = 3L, MessageId = "msg_013", MessageDate = new DateTimeOffset(2024, 1, 1, 15, 10, 0, TimeSpan.Zero), MessageTextId = 13L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 14L, LeadId = 5L, ConsultantId = 3L, MessageId = "msg_014", MessageDate = new DateTimeOffset(2024, 1, 1, 15, 15, 0, TimeSpan.Zero), MessageTextId = 14L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 6 - Juliana (Lead perdido) - Pedro Oliveira
                new { Id = 15L, LeadId = 6L, ConsultantId = 3L, MessageId = "msg_015", MessageDate = new DateTimeOffset(2024, 1, 1, 16, 0, 0, TimeSpan.Zero), MessageTextId = 15L, Sender = MessageSender.customer, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 16L, LeadId = 6L, ConsultantId = 3L, MessageId = "msg_016", MessageDate = new DateTimeOffset(2024, 1, 1, 16, 5, 0, TimeSpan.Zero), MessageTextId = 16L, Sender = MessageSender.consultant, Status = MessageStatus.Read, Type = MessageType.Text, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed LastMessages (última mensagem de cada lead)
            modelBuilder.Entity<LastMessage>().HasData(
                new { LeadId = 1L, TimelineId = 3L, LastMessageText = "Perfeito! Qual seria o melhor horário para a demonstração? Preciso de algo que funcione bem para minha equipe de 5 pessoas.", LastMessageDate = new DateTimeOffset(2024, 1, 1, 10, 10, 0, TimeSpan.Zero), Status = (short)3 },
                new { LeadId = 2L, TimelineId = 6L, LastMessageText = "Sim, por favor! E qual seria o prazo para desenvolvimento? Preciso lançar em 2 meses.", LastMessageDate = new DateTimeOffset(2024, 1, 1, 11, 10, 0, TimeSpan.Zero), Status = (short)3 },
                new { LeadId = 3L, TimelineId = 10L, LastMessageText = "Excelente! Confirmado para quinta às 14h. Vou preparar uma apresentação com nosso plano de transformação digital. Até lá!", LastMessageDate = new DateTimeOffset(2024, 1, 1, 14, 15, 0, TimeSpan.Zero), Status = (short)3 },
                new { LeadId = 4L, TimelineId = 10L, LastMessageText = "Excelente! Confirmado para quinta às 14h. Vou preparar uma apresentação com nosso plano de transformação digital. Até lá!", LastMessageDate = new DateTimeOffset(2024, 1, 1, 14, 15, 0, TimeSpan.Zero), Status = (short)3 },
                new { LeadId = 5L, TimelineId = 14L, LastMessageText = "Sim! Já desenvolvemos 3 apps de delivery. Posso enviar os cases e agendar uma call para detalhar o projeto?", LastMessageDate = new DateTimeOffset(2024, 1, 1, 15, 15, 0, TimeSpan.Zero), Status = (short)3 },
                new { LeadId = 6L, TimelineId = 16L, LastMessageText = "Sem problemas, Juliana! Se precisar de algo no futuro, estaremos aqui. Boa sorte com o projeto!", LastMessageDate = new DateTimeOffset(2024, 1, 1, 16, 5, 0, TimeSpan.Zero), Status = (short)3 }
            );

            // Seed SalesPipelines (pipelines de vendas para cada empresa)
            modelBuilder.Entity<SalesPipeline>().HasData(
                new { Id = 1L, CompanyId = 1L, ConsultantId = 1L, Name = "Pipeline CRM - TechCorp", Position = 1, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, CompanyId = 2L, ConsultantId = 2L, Name = "Pipeline Consultoria - Inovação Digital", Position = 1, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, CompanyId = 3L, ConsultantId = 3L, Name = "Pipeline Apps - StartupHub", Position = 1, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed PipelineStages (estágios do pipeline)
            modelBuilder.Entity<PipelineStage>().HasData(
                // Pipeline 1 - TechCorp (CRM)
                new { Id = 1L, Title = "Novos Leads", Position = 1, SalesPipelineId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 2L, Title = "Qualificados", Position = 2, SalesPipelineId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 3L, Title = "Proposta Enviada", Position = 3, SalesPipelineId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 4L, Title = "Negociação", Position = 4, SalesPipelineId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 5L, Title = "Fechado", Position = 5, SalesPipelineId = 1L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },

                // Pipeline 2 - Inovação Digital (Consultoria)
                new { Id = 6L, Title = "Interesse Inicial", Position = 1, SalesPipelineId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 7L, Title = "Reunião Agendada", Position = 2, SalesPipelineId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 8L, Title = "Apresentação", Position = 3, SalesPipelineId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 9L, Title = "Proposta", Position = 4, SalesPipelineId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 10L, Title = "Fechado", Position = 5, SalesPipelineId = 2L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },

                // Pipeline 3 - StartupHub (Apps)
                new { Id = 11L, Title = "Contato Inicial", Position = 1, SalesPipelineId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 12L, Title = "Briefing", Position = 2, SalesPipelineId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 13L, Title = "Orçamento", Position = 3, SalesPipelineId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 14L, Title = "Desenvolvimento", Position = 4, SalesPipelineId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new { Id = 15L, Title = "Entregue", Position = 5, SalesPipelineId = 3L, CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );

            // Seed LeadStages (posicionamento dos leads nos estágios)
            modelBuilder.Entity<LeadStage>().HasData(
                // Lead 1 (Carlos - CRM) - Estágio "Qualificados"
                new { Id = 1L, LeadId = 1L, PipelineStageId = 2L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 10, 10, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 2 (Ana - E-commerce) - Estágio "Negociação"
                new { Id = 2L, LeadId = 2L, PipelineStageId = 4L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 11, 10, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 3 (Roberto - Consultoria) - Estágio "Reunião Agendada"
                new { Id = 3L, LeadId = 3L, PipelineStageId = 7L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 14, 15, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 4 (Maria - Consultoria) - Estágio "Fechado"
                new { Id = 4L, LeadId = 4L, PipelineStageId = 10L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 14, 15, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 5 (Pedro - App Mobile) - Estágio "Briefing"
                new { Id = 5L, LeadId = 5L, PipelineStageId = 12L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 15, 15, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                
                // Lead 6 (Juliana - Lead perdido) - Estágio "Contato Inicial" (perdido)
                new { Id = 6L, LeadId = 6L, PipelineStageId = 11L, Position = 1, MovedAt = new DateTimeOffset(2024, 1, 1, 16, 5, 0, TimeSpan.Zero), CreatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero), UpdatedAt = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) }
            );
        }
    }
}
