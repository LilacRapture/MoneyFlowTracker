namespace MoneyFlowTracker.Business.Tests.Business.Domain.Chart.Services;

using FluentAssertions;
using MoneyFlowTracker.Business.Domain.Category;
using MoneyFlowTracker.Business.Domain.Chart;
using MoneyFlowTracker.Business.Domain.Chart.Services;
using MoneyFlowTracker.Business.Domain.Item;
using System;
using System.Collections.Generic;

public class AnalyticsChartBuilderTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var category1 = new CategoryModel
        {
            Id = Categories.Revenue,
            Name = "Category 1",
            ParentCategoryId = null,
        };
        var category2 = new CategoryModel
        {
            Id = Guid.Parse("ef340c0e-d636-40a9-97a0-7ef901ca1633"),
            Name = "Category 2",
            ParentCategoryId = category1.Id,
        };
        var category3 = new CategoryModel
        {
            Id = Guid.Parse("5b414fc6-443d-4471-ab11-fe000d77d452"),
            Name = "Category 3",
            ParentCategoryId = category2.Id,
        };

        var item1 = new ItemModel
        {
            Id = Guid.Parse("843e18c6-b6e2-487d-9ea8-b1ea9061656f"),
            AmountCents = 10,
            Name = "Item 1",
            CreatedDate = new DateOnly(2024, 2, 1),
            CategoryId = category1.Id,
            Category = category1,
        };
        var item2 = new ItemModel
        {
            Id = Guid.Parse("0601eea1-92a4-47fd-bc77-981427218dba"),
            AmountCents = 10,
            Name = "Item 2",
            CreatedDate = new DateOnly(2024, 2, 2),
            CategoryId = category1.Id,
            Category = category1,
        };
        var item3 = new ItemModel
        {
            Id = Guid.Parse("30a3a9f1-c91c-4ae5-8687-58f1747d2eb5"),
            AmountCents = 100,
            Name = "Item 3",
            CreatedDate = new DateOnly(2024, 2, 1),
            CategoryId = category2.Id,
            Category = category2,
        };
        var item4 = new ItemModel
        {
            Id = Guid.Parse("14980e98-9848-4493-ace8-c39b6f79ff46"),
            AmountCents = 100,
            Name = "Item 4",
            CreatedDate = new DateOnly(2024, 2, 2),
            CategoryId = category2.Id,
            Category = category2,
        };
        var item5 = new ItemModel
        {
            Id = Guid.Parse("a0a31ffa-58fe-4500-b46f-7b090e1dd17c"),
            AmountCents = 100,
            Name = "Item 5",
            CreatedDate = new DateOnly(2024, 2, 1),
            CategoryId = category3.Id,
            Category = category3,
        };
        var item6 = new ItemModel
        {
            Id = Guid.Parse("5ece18e4-76f3-4bae-84f8-8b30fa9297c8"),
            AmountCents = 100,
            Name = "Item 6",
            CreatedDate = new DateOnly(2024, 2, 2),
            CategoryId = category3.Id,
            Category = category3,
        };
        var item7 = new ItemModel
        {
            Id = Guid.Parse("9969c4f7-3233-452a-9c84-cce9a7594391"),
            AmountCents = 10,
            Name = "Item 7",
            CreatedDate = new DateOnly(2024, 2, 14),
            CategoryId = category1.Id,
            Category = category1,
        };
        var item8 = new ItemModel
        {
            Id = Guid.Parse("f35eb208-c472-42b9-a579-97beccb47ded"),
            AmountCents = 100,
            Name = "Item 8",
            CreatedDate = new DateOnly(2024, 2, 14),
            CategoryId = category2.Id,
            Category = category2,
        };
        var item9 = new ItemModel
        {
            Id = Guid.Parse("d933327d-fb29-4005-94a3-a2b73db2e9ee"),
            AmountCents = 100,
            Name = "Item 9",
            CreatedDate = new DateOnly(2024, 2, 14),
            CategoryId = category3.Id,
            Category = category3,
        };
        var item10 = new ItemModel
        {
            Id = Guid.Parse("92971a9d-31e3-4ec7-b43a-cd219d5d0f4a"),
            AmountCents = 10,
            Name = "Item 10",
            CreatedDate = new DateOnly(2024, 1, 14),
            CategoryId = category1.Id,
            Category = category1,
        };
        var item11 = new ItemModel
        {
            Id = Guid.Parse("6bc85c84-dd36-4036-b701-b0f42cf07d3c"),
            AmountCents = 100,
            Name = "Item 11",
            CreatedDate = new DateOnly(2024, 1, 14),
            CategoryId = category2.Id,
            Category = category2,
        };
        var item12 = new ItemModel
        {
            Id = Guid.Parse("0f398b98-4c23-4f9e-b127-c3c1606216ec"),
            AmountCents = 100,
            Name = "Item 12",
            CreatedDate = new DateOnly(2024, 1, 14),
            CategoryId = category3.Id,
            Category = category3,
        };
        var categoryList = new List<CategoryModel> { category1, category2, category3 };
        var itemList = new List<ItemModel> { item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, item11, item12 };
        var expectedAnalyticsWeeks1 = new List<AnalyticsChartPoint> 
        {
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 7)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 210,
                StartDate = new DateOnly(2024, 1, 8),
                EndDate = new DateOnly(2024, 1, 14)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 15),
                EndDate = new DateOnly(2024, 1, 21)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 22),
                EndDate = new DateOnly(2024, 1, 28)
            },
            new AnalyticsChartPoint 
            {
                AmountCents = 420,
                StartDate = new DateOnly(2024, 1, 29),
                EndDate = new DateOnly(2024, 2, 4)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 2, 5),
                EndDate = new DateOnly(2024, 2, 11)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 210,
                StartDate = new DateOnly(2024, 2, 12),
                EndDate = new DateOnly(2024, 2, 18)
            }
        };
        var expectedAnalyticsMonths1 = new List<AnalyticsChartPoint> 
        {
            new AnalyticsChartPoint
            {
                AmountCents = 210,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 31)
            },
            new AnalyticsChartPoint 
            {
                AmountCents = 630,
                StartDate = new DateOnly(2024, 2, 1),
                EndDate = new DateOnly(2024, 2, 29)
            }
        };
        var expectedAnalyticsQuarters1 = new List<AnalyticsChartPoint> 
        {
            new AnalyticsChartPoint 
            {
                AmountCents = 840,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 3, 31)
            }
        };

        var expectedAnalyticsWeeks2 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 7)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 200,
                StartDate = new DateOnly(2024, 1, 8),
                EndDate = new DateOnly(2024, 1, 14)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 15),
                EndDate = new DateOnly(2024, 1, 21)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 22),
                EndDate = new DateOnly(2024, 1, 28)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 400,
                StartDate = new DateOnly(2024, 1, 29),
                EndDate = new DateOnly(2024, 2, 4)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 2, 5),
                EndDate = new DateOnly(2024, 2, 11)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 200,
                StartDate = new DateOnly(2024, 2, 12),
                EndDate = new DateOnly(2024, 2, 18)
            }
        };
        var expectedAnalyticsMonths2 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 200,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 31)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 600,
                StartDate = new DateOnly(2024, 2, 1),
                EndDate = new DateOnly(2024, 2, 29)
            }
        };
        var expectedAnalyticsQuarters2 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 800,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 3, 31)
            }
        };

        var expectedAnalyticsWeeks3 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 7)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 100,
                StartDate = new DateOnly(2024, 1, 8),
                EndDate = new DateOnly(2024, 1, 14)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 15),
                EndDate = new DateOnly(2024, 1, 21)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 1, 22),
                EndDate = new DateOnly(2024, 1, 28)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 200,
                StartDate = new DateOnly(2024, 1, 29),
                EndDate = new DateOnly(2024, 2, 4)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 0,
                StartDate = new DateOnly(2024, 2, 5),
                EndDate = new DateOnly(2024, 2, 11)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 100,
                StartDate = new DateOnly(2024, 2, 12),
                EndDate = new DateOnly(2024, 2, 18)
            }
        };
        var expectedAnalyticsMonths3 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 100,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 1, 31)
            },
            new AnalyticsChartPoint
            {
                AmountCents = 300,
                StartDate = new DateOnly(2024, 2, 1),
                EndDate = new DateOnly(2024, 2, 29)
            }
        };
        var expectedAnalyticsQuarters3 = new List<AnalyticsChartPoint>
        {
            new AnalyticsChartPoint
            {
                AmountCents = 400,
                StartDate = new DateOnly(2024, 1, 1),
                EndDate = new DateOnly(2024, 3, 31)
            }
        };


        var expectedAnalyticsChart1 = new AnalyticsChart
        {
            Category = new CategoryModel 
            {
                Id = Categories.Revenue,
                Name = "Category 1",
                ParentCategoryId = null,
            },
            Weeks = expectedAnalyticsWeeks1,
            Months = expectedAnalyticsMonths1,
            Quarters = expectedAnalyticsQuarters1,
        };
        var expectedAnalyticsChart2 = new AnalyticsChart
        {
            Category = new CategoryModel
            {
                Id = Guid.Parse("ef340c0e-d636-40a9-97a0-7ef901ca1633"),
                Name = "Category 2",
                ParentCategoryId = Categories.Revenue,
            },
            Weeks = expectedAnalyticsWeeks2,
            Months = expectedAnalyticsMonths2,
            Quarters = expectedAnalyticsQuarters2,
        };
        var expectedAnalyticsChart3 = new AnalyticsChart
        {
            Category = new CategoryModel
            {
                Id = Guid.Parse("5b414fc6-443d-4471-ab11-fe000d77d452"),
                Name = "Category 3",
                ParentCategoryId = Guid.Parse("ef340c0e-d636-40a9-97a0-7ef901ca1633"),
            },
            Weeks = expectedAnalyticsWeeks3,
            Months = expectedAnalyticsMonths3,
            Quarters = expectedAnalyticsQuarters3,
        };

        var expectedAnalyticsCharts = new List<AnalyticsChart> { expectedAnalyticsChart1, expectedAnalyticsChart2, expectedAnalyticsChart3 };

        var actualAnalyticsCharts = new AnalyticsChartBuilder().Build(itemList, categoryList, new DateOnly(2024, 2, 15));
        actualAnalyticsCharts.Should().BeEquivalentTo(expectedAnalyticsCharts, options => options.ComparingByMembers<AnalyticsChart>());
    }
}
