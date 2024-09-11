import { Address } from "./adress.dto";
import { OrderItem } from "./orderItem.dto";

export interface Order {
  id: number;
  totalPrice: number;
  userId: number;
  createdDate: string;
  address: Address;
  orderItems: OrderItem[];
}
