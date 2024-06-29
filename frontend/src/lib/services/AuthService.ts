import type { User } from "../data/User";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tAuthService = Symbol("authService");
@Injectable()
export class AuthService {
  user: User;

  constructor(@Inject(tHttpModule) private http: HttpModule) {
    console.log("created from sl");
  }

  async login(credentials: { username; password }) {
    let logInURL = this.http.url("users/login");

    const res = await this.http.post(logInURL, credentials);
    return res.data;
  }

  async currentUser(): Promise<User> {
    if (!this.user) {
      const me: User = (
        await this.http.get(this.http.url("users/me")).catch(() => {
          return null;
        })
      )?.data;
      this.user = me;
      return me;
    } else {
      return this.user;
    }
  }

  async createAdmin() {
    return this.http.post("/users/registerAdmin", null)
  }
}
