export interface Expense {
  id: string;
  name: string;
  amount: number;
  paymentMethod: number;
  type: number;
  expenseDate: string;
  categoryId?: string;
  categoryName?: string;
}
