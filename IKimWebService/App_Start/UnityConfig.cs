using IKimWebService.Infrastructure.IRepository;
using IKimWebService.Infrastructure.IService;
using IKimWebService.Repository;
using IKimWebService.Service;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace IKimWebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //Service
            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IEmailSenderService, EmailSenderService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IFulfillmentLogService, FulfillmentLogService>();
            container.RegisterType<IItemPriceService, ItemPriceService>();
            container.RegisterType<IItemService, ItemService>();
            container.RegisterType<IJWTokenService, JWTokenService>();
            container.RegisterType<IOrderLineService, OrderLineService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IPricingTierService, PricingTierService>();
            container.RegisterType<IStockItemService, StockItemService>();
            container.RegisterType<IStockService, StockService>();
            container.RegisterType<IStoreService, StoreService>();
            container.RegisterType<IStoreTypeService, StoreTypeService>();
            container.RegisterType<IUnitOfMeasureService, UnitOfMeasureService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUserTypeService, UserTypeService>();

            //Repository
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IFulfillmentLogRepository, FulfillmentLogRepository>();
            container.RegisterType<IItemPriceRepository, ItemPriceRepository>();
            container.RegisterType<IItemRepository, ItemRepository>();
            container.RegisterType<IOrderLineRepository, OrderLineRepository>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IPricingTierRepository, PricingTierRepository>();
            container.RegisterType<IStockItemRepository, StockItemRepository>();
            container.RegisterType<IStockRepository, StockRepository>();
            container.RegisterType<IStoreRepository, StoreRepository>();
            container.RegisterType<IStoreTypeRepository, StoreTypeRepository>();
            container.RegisterType<IUnitOfMeasureRepository, UnitOfMeasureRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserTypeRepository, UserTypeRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

        }
    }
}