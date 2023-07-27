﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

[Table("CartItem")]
public class CartItem: BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cart_item_id")]
    public int CartItemId { get; set; }
    
    [Column("cart_id")]
    public int CartId { get; set; }
    
    [ForeignKey("CartId")]
    public Cart Cart { get; set; }
    
    [Column("product_id")]
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public Product Product { get; set; }
    
    [Required]
    [Column("total_price", TypeName = "money")]
    public decimal TotalPrice { get; set; }
    
    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }
}