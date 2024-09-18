import { Component } from '@angular/core';
import { CreateProductDto } from '../../../models/createProduct.dto';
import { ProductService } from '../../../services/product/product.service';
import { CategoryService } from '../../../services/category/category.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.css']
})
export class ProductCreateComponent {
  product: CreateProductDto = {
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
    categoryId: 0
  };
  categories: any[] = [];
  selectedFile: File | null = null;

  constructor(private categoryService: CategoryService, private productService: ProductService, private router: Router) { }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe((data: any[]) => {
      this.categories = data;
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onSubmit(): void {
    const formData = new FormData();
    formData.append('name', this.product.name);
    formData.append('description', this.product.description);
    formData.append('price', this.product.price.toString());
    formData.append('stockQuantity', this.product.stockQuantity.toString());
    formData.append('categoryId', this.product.categoryId.toString());

    if (this.selectedFile) {
      formData.append('file', this.selectedFile);
    }

    this.productService.createProduct(formData).subscribe(response => {
      console.log('Product created!', response);
      this.router.navigate(['/product/all'])
    });
  }
}
