import { ProductDto } from "./product.dto";

export interface CartItem {
  id: number;
  quantity: number;
  productId: number;
  product: ProductDto;
}
