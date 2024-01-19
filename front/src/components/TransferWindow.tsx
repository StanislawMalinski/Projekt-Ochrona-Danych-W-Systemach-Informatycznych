import '../styles/TransferWindow.css'

interface TransferWindowProps {
    state: boolean;
    close: (arg: void) => void;
}

function TransferWindow(prop: TransferWindowProps) {
    const {state, close} = prop;

    return ( state ?
        <>
            <div className="transfer-window">
                <div className="transfer-window-inner">
                    <h2>New Transfer</h2>
                    <form className='transfer-form'>
                        <input className='transfer-form-input' name="recipent" type='text' placeholder='recipent account number'></input>
                        <input className='transfer-form-input' name="value" type='text' placeholder='money value to transfer'></input>   
                        <button className='transfer-form-input' name="submit" type='submit'>Transfer Money</button>
                        <button className='transfer-form-input' name='cancle' type='button' onClick={() => close()}>Cancle</button>
                    </form>
                </div> 
            </div>   
        </> 
        :
        <></>
    )
}

export default TransferWindow;