using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

public interface IReviewRepository: IRepository<Review>
{
    Task<IEnumerable<Review>> Filter(int? userId, int page, int size);
}