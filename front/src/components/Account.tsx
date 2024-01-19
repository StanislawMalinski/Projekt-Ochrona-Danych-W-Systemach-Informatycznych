import TransferWindow from "./TransferWindow";


function Account() {
    var response = {
        "AccountNumber": "123456789", 
        "Balance": "1000.00", 
        "Currency": "USD", 
        "History": [
            {"AccountNumber": "123456789", "RecipentAccountNumber": "987654321", "Value": "1000.00"},
            {"AccountNumber": "987654321", "RecipentAccountNumber": "123456789", "Value": "100.00"}
        ]};

        function newTransfer() {
            
        }

    return (
        <div>
            <h1>Account</h1>
            <p>Account Number: {response.AccountNumber}</p>
            <p>Balance: {response.Balance}</p>
            <p>Currency: {response.Currency}</p>
            <table>
                <tr>
                    <th>
                        Sender
                    </th>
                    <th>
                        Recipent
                    </th>
                    <th>
                        Value
                    </th>
                </tr>

                {response.History.map((item) =>
                <tr>
                    <td>
                        {item.AccountNumber}
                    </td>    
                    <td>
                        {item.RecipentAccountNumber}
                    </td>
                    <td>
                        {item.Value}
                    </td>
                </tr>
                )}
            </table>

            <TransferWindow/>
                
            <button onClick={newTransfer}>New Transfer</button>
        </div>
    )
}

export default Account;