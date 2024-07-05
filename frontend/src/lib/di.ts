import { Container, LifeTime } from "container-ioc";
import { AuthService, tAuthService } from "./services/AuthService";
import { HttpModule, tHttpModule } from "./services/http";
import { StoreService, tStoreService } from "./services/StoreService";
import { ItemService, tItemService } from "./services/ItemService";
import { tVendorService, VendorService } from "./services/vendorService";
import { CustomerService, tCustomerService } from "./services/CustomerService";
import { InventoryReviewService, tInventoryReviewService } from "./services/InventoryReviewService";

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
    token: tInventoryReviewService,
    useClass: InventoryReviewService,
  },
  {
    token: tHttpModule,
    useClass: HttpModule,
  },
  { token: tStoreService, useClass: StoreService },
]);
