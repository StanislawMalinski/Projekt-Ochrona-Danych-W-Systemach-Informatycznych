import { transfer } from '../Client';
import '../styles/TransferWindow.css'
import { useState } from 'react';

interface TransferWindowProps {
    state: boolean;
    accountNumber: string;
    relod: (arg: void) => void;
    close: (arg: void) => void;

}

function TransferWindow(prop: TransferWindowProps) {
    const {state, close, accountNumber, relod} = prop;

    const [recipientAccountNumber, setRecipentAccountNumber] = useState("");
    const [recipient, setRecipent] = useState("");
    const [value, setValue] = useState(0);
    const [title, setTitle] = useState("");
    //const [error, setError] = useState("");

    const actionTransfer = () => {
        transfer({accountNumber: accountNumber, recipientAccountNumber: recipientAccountNumber, recipient: recipient, value: value, title: title})
        .then((response) => {
            if (response) {
                (response)
                if (response.success) {
                    relod();
                    close();
                }
            }
        });

    }



    return ( state ?
        <>
            <div className="transfer-window">
                <div className="transfer-window-inner">
                    <h2>New Transfer</h2>
                    <form className='transfer-form'>
                        <input className='transfer-form-input' 
                            name="recipient-number" type='text' placeholder='recipient account number' 
                            onChange={(e) => setRecipentAccountNumber(e.target.value)}></input>
                        <input className='transfer-form-input' 
                            name="recipient" type='text' placeholder='recipient name' 
                            onChange={(e) => setRecipent(e.target.value)}></input>
                        <input className='transfer-form-input' 
                            name="value" type='text' placeholder='money value to transfer'
                            onChange={(e) => setValue(Number(e.target.value))}></input>  
                        <input className='transfer-form-input'
                            name="title" type='text' placeholder='title'
                            onChange={(e) => setTitle(e.target.value)}></input> 
                    </form>
                
                    <button className='transfer-form-input' name="submit" type='button' onClick={actionTransfer}>Transfer Money</button>
                    <button className='transfer-form-input' name='cancle' type='button' onClick={() => close()}>Cancle</button>
                </div> 
            </div>   
        </> 
        :
        <></>
    )
}

export default TransferWindow;