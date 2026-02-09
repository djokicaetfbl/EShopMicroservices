namespace Shopping.Web.Pages
{
    public class CheckoutModel(IBasketService basketService, ILogger<CheckoutModel> logger) : PageModel
    {
        [BindProperty]
        public BasketCheckoutModel Order { get; set; } = default!;
        public ShoppingCartModel Cart { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.LoadUserBasket();
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            logger.LogInformation("Checkout button clicked");

            Cart = await basketService.LoadUserBasket();

            if(!ModelState.IsValid)
            {
                logger.LogWarning("Checkout form is invalid");
                return Page();
            }

            // assumption is passed in from the UI authenticated user swn
            Order.CustomerId = new Guid("bfb9c8e7-1c9d-4a3b-9f0e-2a5d6c7e8f9a");
            Order.UserName = Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;

            await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
