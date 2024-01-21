import '../styles/Account.css';

interface AccountProps {
    newTransfer: (arg: void) => void;
    credentials: any;
    logOut: (arg: void) => void;
}

function Account(props: AccountProps) {
    const {newTransfer, logOut, credentials} = props;
    
    return (credentials ? 
        <>
            <h1>Account</h1>
            <p>Account Number: {credentials.accountNumber}</p>
            <p>Balance: {credentials.balance.toFixed(2)}</p>

            <div className="transfer-history-container">
                <table className="transfer-history">
                    <tbody>
                        <tr>
                            <th className="transfer-history-cell">
                                Sender
                            </th>
                            <th className="transfer-history-cell">
                                Recipent
                            </th>
                            <th className="transfer-history-cell">
                                Value
                            </th>
                        </tr>

                        {credentials.history.map((item: any, index: number) =>
                        <tr key={index}>
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.accountNumber}</p>
                            </td>    
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.recipent ? item.recipent : item.recipentAccountNumber}</p>
                            </td>
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.title}</p>
                            </td>
                            <td className="transfer-history-cell transfer-value">
                                <p className={item.accountNumber === credentials.accountNumber 
                                    ? 'taken' : 'received'}>
                                    {item.accountNumber === credentials.accountNumber ? '-':''}{item.value.toFixed(2)}
                                </p>
                            </td>
                        </tr>
                        )}
                    </tbody>
                </table>
            </div>
            <button onClick={() => newTransfer()}>New Transfer</button>
            <button onClick={() => logOut()}>LogOut</button>
        </>:
        <>  </>
    )
}

export default Account;