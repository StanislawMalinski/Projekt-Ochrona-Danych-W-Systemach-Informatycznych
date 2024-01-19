import PasswordInput from "./PasswordInput";

interface AccountProps {
    newTransfer: (arg: void) => void;
    logOut: (arg: void) => void;
}

function Account(props: AccountProps) {
    const {newTransfer, logOut} = props;
    var response = {
        "AccountNumber": "123456789", 
        "Balance": "1000.00", 
        "Currency": "USD", 
        "History": [
            {"AccountNumber": "123456789", "RecipentAccountNumber": "987654321", "Value": "1000.00"},
            {"AccountNumber": "987654321", "RecipentAccountNumber": "123456789", "Value": "100.00"}
        ]};

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
            <button onClick={() => newTransfer()}>New Transfer</button>
            <button onClick={() => logOut()}>LogOut</button>
        </div>
    )
}

export default Account;