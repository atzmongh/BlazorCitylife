using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorCitylife.Models
{
    public partial class citylifedb8_blContext : DbContext
    {
        public citylifedb8_blContext()
        {
        }

        public citylifedb8_blContext(DbContextOptions<citylifedb8_blContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apartment> Apartment { get; set; }
        public virtual DbSet<ApartmentDay> ApartmentDay { get; set; }
        public virtual DbSet<ApartmentPhoto> ApartmentPhoto { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }
        public virtual DbSet<CurrencyExchange> CurrencyExchange { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeWorkDay> EmployeeWorkDay { get; set; }
        public virtual DbSet<ErrorCode> ErrorCode { get; set; }
        public virtual DbSet<ErrorMessage> ErrorMessage { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<ExpenseType> ExpenseType { get; set; }
        public virtual DbSet<Guest> Guest { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Pricing> Pricing { get; set; }
        public virtual DbSet<Translation> Translation { get; set; }
        public virtual DbSet<TranslationKey> TranslationKey { get; set; }
        public virtual DbSet<UnitTest> UnitTest { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (/*!optionsBuilder.IsConfigured*/true)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                 optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=citylifedb8_bl;Trusted_Connection=True;");
               // optionsBuilder.UseSqlServer("Data Source=citylife4dbserver.database.windows.net;Initial Catalog=db_cityLife4;User ID=atzmon.ghilai;Password=TelAviv22;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>(entity =>
            {
                entity.Property(e => e.AddressKey)
                    .IsRequired()
                    .HasColumnName("addressKey");

                entity.Property(e => e.DescriptionKey)
                    .IsRequired()
                    .HasColumnName("descriptionKey");

                entity.Property(e => e.FeaturesKeys).HasColumnName("featuresKeys");

                entity.Property(e => e.NameKey)
                    .IsRequired()
                    .HasColumnName("nameKey");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<ApartmentDay>(entity =>
            {
                entity.HasIndex(e => e.ApartmentId)
                    .HasName("IX_FK_ApartmentApartmentDay");

                entity.HasIndex(e => e.CurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyApartmentDay");

                entity.HasIndex(e => new { e.ApartmentId, e.Date })
                    .HasName("IX_DATE");

                entity.Property(e => e.ApartmentId).HasColumnName("Apartment_Id");

                entity.Property(e => e.CurrencyCurrencyCode)
                    .HasColumnName("Currency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsCleaned).HasColumnName("isCleaned");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.PriceFactor)
                    .HasColumnName("priceFactor")
                    .HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Revenue).HasColumnName("revenue");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.ApartmentDays)
                    .HasForeignKey(d => d.ApartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApartmentApartmentDay");

                entity.HasOne(d => d.CurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.ApartmentDays)
                    .HasForeignKey(d => d.CurrencyCurrencyCode)
                    .HasConstraintName("FK_CurrencyApartmentDay");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ApartmentDays)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderApartmentDay");
            });

            modelBuilder.Entity<ApartmentPhoto>(entity =>
            {
                entity.HasIndex(e => e.ApartmentId)
                    .HasName("IX_FK_ApartmentApartmentPhoto");

                entity.Property(e => e.ApartmentId).HasColumnName("Apartment_Id");

                entity.Property(e => e.FilePath)
                    .IsRequired()
                    .HasColumnName("filePath");

                entity.Property(e => e.ForDesktop).HasColumnName("forDesktop");

                entity.Property(e => e.ForMobile).HasColumnName("forMobile");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Orientation).HasColumnName("orientation");

                entity.Property(e => e.SortOrder).HasColumnName("sortOrder");

                entity.Property(e => e.ThumbnailPath).HasColumnName("thumbnailPath");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.ApartmentPhotoes)
                    .HasForeignKey(d => d.ApartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApartmentApartmentPhoto");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Countries");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Language)
                    .HasColumnName("language")
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(e => e.CurrencyCode)
                    .HasName("PK_Currencies");

                entity.Property(e => e.CurrencyCode)
                    .HasColumnName("currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Symbol)
                    .IsRequired()
                    .HasColumnName("symbol")
                    .HasMaxLength(1)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CurrencyExchange>(entity =>
            {
                entity.HasIndex(e => e.FromCurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyCurrencyExchangeFrom");

                entity.HasIndex(e => e.ToCurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyCurrencyExchangeTo");

                entity.Property(e => e.ConversionRate)
                    .HasColumnName("conversionRate")
                    .HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.FromCurrencyCurrencyCode)
                    .IsRequired()
                    .HasColumnName("FromCurrency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.ToCurrencyCurrencyCode)
                    .IsRequired()
                    .HasColumnName("ToCurrency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.HasOne(d => d.FromCurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.CurrencyExchangeFromCurrencyCurrencyCodeNavigation)
                    .HasForeignKey(d => d.FromCurrencyCurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyCurrencyExchangeFrom");

                entity.HasOne(d => d.ToCurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.CurrencyExchangeToCurrencyCurrencyCodeNavigation)
                    .HasForeignKey(d => d.ToCurrencyCurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyCurrencyExchangeTo");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName");
            });

            modelBuilder.Entity<EmployeeWorkDay>(entity =>
            {
                entity.HasIndex(e => e.CurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyEmployeeWorkDay");

                entity.HasIndex(e => e.DateAndTime)
                    .HasName("IX_DATE");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("IX_FK_EmployeeEmployeeWorkDay");

                entity.Property(e => e.CurrencyCurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.DateAndTime)
                    .HasColumnName("dateAndTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

                entity.Property(e => e.Hours).HasColumnName("hours");

                entity.Property(e => e.IsSalaryDay).HasColumnName("isSalaryDay");

                entity.Property(e => e.SalaryCents).HasColumnName("salaryCents");

                entity.HasOne(d => d.CurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.EmployeeWorkDays)
                    .HasForeignKey(d => d.CurrencyCurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyEmployeeWorkDay");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeWorkDays)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeEmployeeWorkDay");
            });

            modelBuilder.Entity<ErrorCode>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_ErrorCodes");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastOccurenceDate)
                    .HasColumnName("lastOccurenceDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnName("message");

                entity.Property(e => e.OccurenceCount).HasColumnName("occurenceCount");
            });

            modelBuilder.Entity<ErrorMessage>(entity =>
            {
                entity.HasIndex(e => e.ErrorCodeCode)
                    .HasName("IX_FK_ErrorCodeErrorMessage");

                entity.Property(e => e.ErrorCodeCode).HasColumnName("ErrorCode_code");

                entity.Property(e => e.FormattedMessage)
                    .IsRequired()
                    .HasColumnName("formattedMessage");

                entity.Property(e => e.LastOccurenceDate)
                    .HasColumnName("lastOccurenceDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.OccurenceCount).HasColumnName("occurenceCount");

                entity.Property(e => e.StackTrace).HasColumnName("stackTrace");

                entity.HasOne(d => d.ErrorCodeCodeNavigation)
                    .WithMany(p => p.ErrorMessages)
                    .HasForeignKey(d => d.ErrorCodeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ErrorCodeErrorMessage");
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasIndex(e => e.CurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyExpense");

                entity.HasIndex(e => e.Date)
                    .HasName("IX_DATE");

                entity.HasIndex(e => e.ExpenseTypeId)
                    .HasName("IX_FK_ExpenseTypeExpense");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CurrencyCurrencyCode)
                    .HasColumnName("currency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.ExpenseTypeId).HasColumnName("expenseType_Id");

                entity.HasOne(d => d.CurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.CurrencyCurrencyCode)
                    .HasConstraintName("FK_CurrencyExpense");

                entity.HasOne(d => d.ExpenseType)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.ExpenseTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ExpenseTypeExpense");
            });

            modelBuilder.Entity<ExpenseType>(entity =>
            {
                entity.Property(e => e.DescriptionKey).HasColumnName("descriptionKey");

                entity.Property(e => e.NameKey)
                    .IsRequired()
                    .HasColumnName("nameKey")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasIndex(e => e.CountryCode)
                    .HasName("IX_FK_CountryGuest");

                entity.Property(e => e.CountryCode)
                    .HasColumnName("Country_code")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone");

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Guests)
                    .HasForeignKey(d => d.CountryCode)
                    .HasConstraintName("FK_CountryGuest");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.LanguageCode)
                    .HasName("PK_Languages");

                entity.Property(e => e.LanguageCode)
                    .HasColumnName("languageCode")
                    .HasMaxLength(10);

                entity.Property(e => e.IsDefault).HasColumnName("isDefault");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.ApartmentId)
                    .HasName("IX_FK_ApartmentOrder");

                entity.HasIndex(e => e.CurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyOrder");

                entity.HasIndex(e => e.GuestId)
                    .HasName("IX_FK_GuestOrder");

                entity.Property(e => e.AdultCount).HasColumnName("adultCount");

                entity.Property(e => e.AmountPaid).HasColumnName("amountPaid");

                entity.Property(e => e.ApartmentId).HasColumnName("Apartment_Id");

                entity.Property(e => e.BookedBy).HasColumnName("bookedBy");

                entity.Property(e => e.CancellationNumber).HasColumnName("cancellationNumber");

                entity.Property(e => e.CheckinDate)
                    .HasColumnName("checkinDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.CheckoutDate)
                    .HasColumnName("checkoutDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChildrenCount).HasColumnName("childrenCount");

                entity.Property(e => e.ConfirmationNumber)
                    .IsRequired()
                    .HasColumnName("confirmationNumber");

                entity.Property(e => e.CurrencyCurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.DayCount).HasColumnName("dayCount");

                entity.Property(e => e.ExpectedArrival).HasColumnName("expectedArrival");

                entity.Property(e => e.GuestId).HasColumnName("Guest_Id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.SpecialRequest).HasColumnName("specialRequest");

                entity.Property(e => e.StaffComments).HasColumnName("staffComments");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ApartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApartmentOrder");

                entity.HasOne(d => d.CurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CurrencyCurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyOrder");

                entity.HasOne(d => d.Guest)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.GuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuestOrder");
            });

            modelBuilder.Entity<Pricing>(entity =>
            {
                entity.HasIndex(e => e.ApartmentId)
                    .HasName("IX_FK_ApartmentPricing");

                entity.HasIndex(e => e.CurrencyCurrencyCode)
                    .HasName("IX_FK_CurrencyPricing");

                entity.Property(e => e.Adults).HasColumnName("adults");

                entity.Property(e => e.ApartmentId).HasColumnName("Apartment_Id");

                entity.Property(e => e.Children).HasColumnName("children");

                entity.Property(e => e.CurrencyCurrencyCode)
                    .IsRequired()
                    .HasColumnName("Currency_currencyCode")
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.PriceWeekDay).HasColumnName("priceWeekDay");

                entity.Property(e => e.PriceWeekEnd).HasColumnName("priceWeekEnd");

                entity.HasOne(d => d.Apartment)
                    .WithMany(p => p.Pricings)
                    .HasForeignKey(d => d.ApartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApartmentPricing");

                entity.HasOne(d => d.CurrencyCurrencyCodeNavigation)
                    .WithMany(p => p.Pricings)
                    .HasForeignKey(d => d.CurrencyCurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyPricing");
            });

            modelBuilder.Entity<Translation>(entity =>
            {
                entity.HasIndex(e => e.LanguageLanguageCode)
                    .HasName("IX_FK_LanguageTranslation");

                entity.HasIndex(e => e.TranslationKeyId)
                    .HasName("IX_FK_TranslationKeyTranslatio");

                entity.Property(e => e.LanguageLanguageCode)
                    .IsRequired()
                    .HasColumnName("Language_languageCode")
                    .HasMaxLength(10);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message");

                entity.Property(e => e.TranslationKeyId).HasColumnName("TranslationKey_id");

                entity.HasOne(d => d.LanguageLanguageCodeNavigation)
                    .WithMany(p => p.Translations)
                    .HasForeignKey(d => d.LanguageLanguageCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageTranslation");

                entity.HasOne(d => d.TranslationKey)
                    .WithMany(p => p.Translations)
                    .HasForeignKey(d => d.TranslationKeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationKeyTranslatio");
            });

            modelBuilder.Entity<TranslationKey>(entity =>
            {
                entity.HasIndex(e => e.TransKey)
                    .HasName("IX_transKey");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FilePath).HasColumnName("filePath");

                entity.Property(e => e.IsUsed).HasColumnName("isUsed");

                entity.Property(e => e.LineNumber).HasColumnName("lineNumber");

                entity.Property(e => e.TransKey)
                    .HasColumnName("transKey")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<UnitTest>(entity =>
            {
                entity.HasKey(e => new { e.Series, e.Number })
                    .HasName("PK_unitTests");

                entity.ToTable("unitTest");

                entity.Property(e => e.Series)
                    .HasColumnName("series")
                    .HasMaxLength(64);

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.ActualResult).HasColumnName("actualResult");

                entity.Property(e => e.CorrectFlag).HasColumnName("correctFlag");

                entity.Property(e => e.DateLastRun)
                    .HasColumnName("dateLastRun")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpectedResult).HasColumnName("expectedResult");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
