using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Services;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.CategoryRepository).Returns(_mockCategoryRepository.Object);

        _categoryService = new CategoryService(mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { CategoryId = 1, CategoryName = "Category1" },
            new Category { CategoryId = 2, CategoryName = "Category2" }
        };
        var mappedCategories = new List<CategoryGet>
        {
            new CategoryGet { CategoryId = 1, CategoryName = "Category1" },
            new CategoryGet { CategoryId = 2, CategoryName = "Category2" }
        };

        _mockCategoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);
        _mockMapper.Setup(m => m.Map<CategoryGet>(It.IsAny<Category>()))
                   .Returns((Category source) => mappedCategories.First(c => c.CategoryId == source.CategoryId));

        // Act
        var result = await _categoryService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, c => c.CategoryName == "Category1");
        Assert.Contains(result, c => c.CategoryName == "Category2"); 
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category { CategoryId = 1, CategoryName = "Category1" };
        var mappedCategory = new CategoryGet { CategoryId = 1, CategoryName = "Category1" };

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _mockMapper.Setup(m => m.Map<CategoryGet>(category)).Returns(mappedCategory);

        // Act
        var result = await _categoryService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Category1", result.CategoryName);
    }

    [Fact]
    public async Task AddAsync_ShouldCallRepositoryWithMappedEntity()
    {
        // Arrange
        var categoryDto = new CategoryPost { CategoryName = "NewCategory" };
        var category = new Category { CategoryName = "NewCategory" };

        _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);

        // Act
        await _categoryService.AddAsync(categoryDto);

        // Assert
        _mockCategoryRepository.Verify(r => r.AddAsync(category), Times.Once);
        
        Assert.Equal("NewCategory", categoryDto.CategoryName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepositoryDelete()
    {
        // Act
        await _categoryService.DeleteAsync(1);

        // Assert
        _mockCategoryRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}