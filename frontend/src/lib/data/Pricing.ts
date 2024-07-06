export class Pricing{
    id: number;
    startDate: string;
    endDate: string;
    
    constructor(
        startDate: Date,
        endDate: Date,
        id?: number,
      ) {
        this.id = id;
        this.startDate = startDate.toLocaleString();
        this.endDate = endDate.toLocaleString();
      }
}