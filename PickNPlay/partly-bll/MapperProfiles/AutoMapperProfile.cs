using AutoMapper;
using PickNPlay.picknplay_bll.Models.Category;
using PickNPlay.picknplay_bll.Models.Deposit;
using PickNPlay.picknplay_bll.Models.Favourite;
using PickNPlay.picknplay_bll.Models.Game;
using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_bll.Models.Provider;
using PickNPlay.picknplay_bll.Models.Review;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_bll.MapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryGet>().ReverseMap();
            CreateMap<Listing, ListingGet>().ReverseMap();
            CreateMap<Listing, ListingNameAndPriceGet>().ReverseMap();
            CreateMap<Listing, ListingGetWithUserNoReviews>().ReverseMap();
            // CreateMap<Game, GameGet>().ReverseMap();
            // CreateMap<Game, GameWithListingsAndCategoriesGet>().ReverseMap();
            CreateMap<Review, ReviewGet>().ReverseMap();
            CreateMap<ReviewBriefGet, Review>()
                .ForPath(dest => dest.Transaction.Listing, opt => opt.MapFrom(src => src.ListingNameAndPrice))
                .ReverseMap()
                .ForMember(src => src.ListingNameAndPrice, opt => opt.MapFrom(dest => dest.Transaction.Listing));
            CreateMap<Message, MessageGet>().ReverseMap();
            CreateMap<User, UserGet>().ReverseMap();
            CreateMap<User, UserFullInfoGet>().ReverseMap();
            CreateMap<User, UserReallyBriefInfoWithoutReviews>().ReverseMap();
            // CreateMap<AccountProvider, ProviderGet>().ReverseMap();
            CreateMap<Favourite, FavouriteGet>().ReverseMap();
            CreateMap<Transaction, TransactionGet>();
                //.ForMember(dest => dest.Buyer,opt => opt.MapFrom(src => src.Buyer));
            CreateMap<Deposit,DepositGet>().ReverseMap();

            CreateMap<Category, CategoryUpdate>().ReverseMap();
            CreateMap<Listing, ListingUpdate>().ReverseMap();
            // CreateMap<Game, GameUpdate>().ReverseMap();
            CreateMap<Review, ReviewUpdate>().ReverseMap();
            CreateMap<Message, MessageUpdate>().ReverseMap();
            CreateMap<Transaction, TransactionUpdate>().ReverseMap();
            CreateMap<User, UserUpdate>().ReverseMap();
            //CreateMap<AccountProvider, ProviderUpdate>().ReverseMap();
            CreateMap<Deposit, DepositPost>().ReverseMap();




            CreateMap<Category, CategoryPost>().ReverseMap();
            CreateMap<Listing, ListingPost>().ReverseMap();
            //CreateMap<Game, GamePost>().ReverseMap();
            CreateMap<Review, ReviewPost>().ReverseMap();
            CreateMap<Message, MessagePost>().ReverseMap();
            CreateMap<Transaction, TransactionPost>().ReverseMap();
            CreateMap<User, UserPost>().ReverseMap();
            //CreateMap<AccountProvider, ProviderPost>().ReverseMap();
            CreateMap<Favourite, FavouritePost>().ReverseMap();
            CreateMap<Deposit, DepositUpdate>().ReverseMap();


            CreateMap<ListingImage, ListingImageGet>().ReverseMap();
            CreateMap<ListingImage, ListingImagePost>().ReverseMap();




            //CreateMap<IEnumerable<Category>, IEnumerable<Category>>().ReverseMap();
            //CreateMap<IEnumerable<Listing>, IEnumerable<ListingGet>>().ReverseMap();
            //CreateMap<IEnumerable<Game>, IEnumerable<GameGet>>().ReverseMap();
        }
    }
}
