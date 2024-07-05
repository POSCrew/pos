import { Container, LifeTime } from "container-ioc";
import { AuthService, tAuthService } from "./services/AuthService";
import { HttpModule, tHttpModule } from "./services/http";
import { StoreService, tStoreService } from "./services/StoreService";
import { ItemService, tItemService } from "./services/ItemService";
import { tVendorService, VendorService } from "./services/vendorService";
import { CustomerService, tCustomerService } from "./services/CustomerService";
import { InventoryReviewService, tInventoryReviewService } from "./services/InventoryReviewService";
import { SalesReviewService, tSalesReviewService } from "./services/SalesReviewService";
import { PricingService, tPricingService } from "./services/PricingService";

export const sl = new Container({
  defaultLifeTime: LifeTime.Persistent,
});

sl.register([
  {
    token: tAuthService,
    useClass: AuthService,
  },
  {
    token: tStoreService,
    useClass: StoreService,
  },
  {
    token: tVendorService,
    useClass: VendorService,
  },
  {
    token: tCustomerService,
    useClass: CustomerService,
  },
  {
    token: tItemService,
    useClass: ItemService,
  },
  {
    token: tPricingService,
    useClass: PricingService,
  },
  {
    token: tInventoryReviewService,
    useClass: InventoryReviewService,
  },
  {
    token: tSalesReviewService,
    useClass: SalesReviewService,
  },
  {
    token: tHttpModule,
    useClass: HttpModule,
  },
  { token: tStoreService, useClass: StoreService },
]);
