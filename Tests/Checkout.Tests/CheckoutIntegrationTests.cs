using System;
using System.Configuration;
using Autofac;
using Checkout.Interfaces;
using Checkout.Ioc;
using FluentAssertions;
using Xunit;

namespace Checkout.Tests
{
    public class CheckoutIntegrationTests : IDisposable
    {
        private IContainer _container;

        public CheckoutIntegrationTests()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            var builder = new ContainerBuilder();
            builder.RegisterModule(new DatabaseModule(connectionString));
            builder.RegisterModule<ConventionModule>();
            _container = builder.Build();
        }

        [Theory]
        [InlineData("A", 50)]
        [InlineData("B", 30)]
        [InlineData("C", 20)]
        [InlineData("D", 15)]
        [InlineData("A;a;A", 130)]
        public void WhenScanningItemsThenTheTotalPriceShouldBeAsExpected(string items, int expectedTotal)
        {
            using (var innerScope = _container.BeginLifetimeScope())
            {
                // Arrange
                ICheckout checkout = innerScope.Resolve<ICheckout>();

                // Act
                var itemArray = items.Split(';');
                foreach (var item in itemArray)
                {
                    checkout.Scan(item);

                }
                var result = checkout.GetTotalPrice();

                // Assert
                result.Should().Be(expectedTotal);
            }
        }

        public void Dispose()
        {
            _container.Dispose();
            _container = null;
        }
    }
}