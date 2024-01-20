import { useEffect, useState } from "react";
import { account } from "../Client";

import '../styles/Account.css';

interface AccountProps {
    newTransfer: (arg: void) => void;
    logOut: (arg: void) => void;
}

function Account(props: AccountProps) {
    const {newTransfer, logOut} = props;

    const [response, setResponse] = useState({accountNumber: "", balance: 0, history: [{accountNumber: "", recipentAccountNumber: "",recipent:"" ,title:"", value: 0}]});

    
    const accountRequest = {"accountNumber": "123456789"}; 
    
    useEffect(() => {
        account(accountRequest)
            .then((resp) => {setResponse(resp); console.log(resp);})
            .catch((error) => {console.log(error);});
    }, []);

    
    return (
        <>
            <h1>Account</h1>
            <p>Account Number: {response.accountNumber}</p>
            <p>Balance: {response.balance.toFixed(2)}</p>

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

                        {response.history.map((item, index) =>
                        <tr key={index}>
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.accountNumber}</p>
                            </td>    
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.recipentAccountNumber}</p>
                            </td>
                            <td className="transfer-history-cell transfer-value">
                                <p>{item.title}</p>
                            </td>
                            <td className="transfer-history-cell transfer-value">
                                <p className={item.accountNumber === accountRequest.accountNumber 
                                    ? 'taken' : 'received'}>
                                    {item.accountNumber === accountRequest.accountNumber ? '-':''}{item.value.toFixed(2)}
                                </p>
                            </td>
                        </tr>
                        )}
                    </tbody>
                </table>
            </div>
            <button onClick={() => newTransfer()}>New Transfer</button>
            <button onClick={() => logOut()}>LogOut</button>
        </>
    )
}

export default Account;