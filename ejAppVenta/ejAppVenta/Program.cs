// cod  producto   precio
// DES desodorante 200
// JP Jabon en polvo 300
// DET detergente 250

const string Exit = "FIN";
const int DesPrice = 200;
const int JpPrice = 300;
const int DetPrice = 250;



void AddProduct(ref int total, string inputQuantity, string inputCode) {
    int parsedQuantity = int.Parse(inputQuantity);
    if (inputCode.Equals("DES"))
    {
        total += DesPrice * parsedQuantity;
    }
    else if (inputCode.Equals("JP"))
    {
        total += JpPrice * parsedQuantity;
    }
    else if (inputCode.Equals("DET"))
    {
        total += DetPrice * parsedQuantity;
    }
    else {
        Console.WriteLine("Wrong Code");
    }
}

void Confirm()
{
    Console.WriteLine("Confirm purchase? (Entres Y for Yes or N for No)");
    string confirmation = Console.ReadLine();

    if (confirmation.Equals("Y"))
    {
        Console.WriteLine("Thanks for your purchase");
    }
}


void run()
{
    Console.WriteLine("Welcome to sales system");
    Console.WriteLine("Enter product code(Enter FIN to exit): ");
    string inputCode = Console.ReadLine();
    Console.WriteLine("Enter desired quantity: ");
    string inputQuantity = Console.ReadLine();
    int total = 0;

    while (!inputCode.Equals(Exit))
    {

        AddProduct(ref total, inputQuantity, inputCode);

        Console.WriteLine("Enter product code(Enter FIN to exit): ");
        inputCode = Console.ReadLine();
        if (inputCode.Equals(Exit)) { break; }
        Console.WriteLine("Enter desired quantity: ");
        inputQuantity = Console.ReadLine();
    }


    Console.WriteLine("Total price is: $" + total);
    Confirm();

}

run();

