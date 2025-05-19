using PruebaTecnica.EntityFrameworkCore;

namespace PruebaTecnica.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly PruebaTecnicaDbContext _context;

        public TestDataBuilder(PruebaTecnicaDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            //create test data here...
        }
    }
}