import { ProductDto } from "./product.dto";

export interface OrderItem {
    id: number;
    quantity: number;
    productId: number;
    product: ProductDto;
}
