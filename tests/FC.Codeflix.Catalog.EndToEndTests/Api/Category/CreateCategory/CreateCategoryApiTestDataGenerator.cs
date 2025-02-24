using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    public class CreateCategoryApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;
            var input = fixture.GetExampleInput();
            for (int i = 0; i < 3; i++)
            {
                switch (i % totalInvalidCases)
                {
                    case 0:
                        input = fixture.GetExampleInput();
                        input.Name = fixture.GetInvalidNameTooShort();
                        invalidInputsList.Add(new object[] { input, "Name should be at least 3 characters long" });
                        break;
                    case 1:
                        input = fixture.GetExampleInput();
                        input.Name = fixture.GetInvalidNameTooLong();
                        invalidInputsList.Add(new object[] { input, "Name should be less or equal 255 characters long" });
                        break;
                    case 2:
                        input = fixture.GetExampleInput();
                        input.Description = fixture.GetInvalidDescriptionTooLong();
                        invalidInputsList.Add(new object[] { input, "Description should be less or equal 10000 characters long" });
                        break;
                    default:
                        break;
                }
            }
            return invalidInputsList;
        }
    }
}
