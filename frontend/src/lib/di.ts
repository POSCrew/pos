import { Container, LifeTime } from "container-ioc";
import { AuthService, tAuthService } from "./services/AuthService";
import { HttpModule, tHttpModule } from "./services/http";
import { StoreService, tStoreService } from "./services/StoreService";

export const sl = new Container({
  defaultLifeTime: LifeTime.Persistent,
});

sl.register([
  {
    token: tAuthService,
    useClass: AuthService,
  },
  {
    token: tHttpModule,
    useClass: HttpModule,
  },
  { token: tStoreService, useClass: StoreService },
]);
