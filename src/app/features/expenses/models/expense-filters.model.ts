export interface ExpenseFilters {
  search: string;
  type: string;
  paymentMethod: string;
  category: string;
  fromDate: string;
  toDate: string;
  minAmount: number | null;
  maxAmount: number | null;
}
