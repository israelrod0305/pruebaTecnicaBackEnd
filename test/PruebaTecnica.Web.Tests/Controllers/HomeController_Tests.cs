using System.Threading.Tasks;
using PruebaTecnica.Web.Controllers;
using Shouldly;
using Xunit;

namespace PruebaTecnica.Web.Tests.Controllers
{
    public class HomeController_Tests: PruebaTecnicaWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
