import type { InventoryReviewItems } from "../data/InventoryReviewItems";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tInventoryReviewService = Symbol("InventoryReviewService");
@Injectable()
export class InventoryReviewService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async getItemSheetData(itemID:number){
    const url = this.http.url("inventory/review/items")

    return (await this.http.post(url, {itemID})).data as InventoryReviewItems[];
  }
}
