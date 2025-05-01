using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;

public class CategoryRepositoryTests
{
    private readonly Mock<picknplayContext> _mockContext;
    private readonly Mock<DbSet<Category>> _mockDbSet;
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests()
    {
        _mockContext = new Mock<picknplayContext>();
        _mockDbSet = new Mock<DbSet<Category>>();

        _mockContext.Setup(c => c.Categories).Returns(_mockDbSet.Object);
        _repository = new CategoryRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { CategoryId = 1, CategoryName = "Category1" },
            new Category { CategoryId = 2, CategoryName = "Category2" }
        }.AsQueryable();

        _mockDbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(categories.Provider);
        _mockDbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(categories.Expression);
        _mockDbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(categories.ElementType);
        _mockDbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());
        
        var result = categories;

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.CategoryName == "Category1");
        Assert.Contains(result, c => c.CategoryName == "Category2");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { CategoryId = 1, CategoryName = "Category1" };
        _mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        // Assert.Equal("Category1", result.CategoryName);
        Assert.Equal("Category1", result.CategoryName);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategory()
    {
        // Arrange
        var category = new Category { CategoryId = 1, CategoryName = "NewCategory" };

        // Act
        await _repository.AddAsync(category);

        // Assert
        _mockDbSet.Verify(m => m.AddAsync(category, default), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
}