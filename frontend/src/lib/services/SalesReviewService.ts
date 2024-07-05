import type { SalesReviewProfit } from "../data/SalesReviewProfit";
import { HttpModule, tHttpModule } from "./http";
import { Inject, Injectable } from "container-ioc";

export const tSalesReviewService = Symbol("SalesReviewService");
@Injectable()
export class SalesReviewService {
  constructor(@Inject(tHttpModule) private http: HttpModule) {}
  async getItemSheetData(startDate:Date, endDate:Date){
    const url = this.http.url("sales/review/profit")

    return (await this.http.post(url, {startDate, endDate})).data as SalesReviewProfit[];
  }
}
