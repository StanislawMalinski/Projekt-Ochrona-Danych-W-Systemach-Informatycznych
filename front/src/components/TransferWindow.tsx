

function TransferWindow() {
    return (
        <>
            <div className="transfer-window">
                <h2>New Transfer</h2>
                <form>
                    <input name="recipent" type='text'></input>
                    <input name="value" type='text'></input>   
                </form>
            </div>   
        </>
    )
}

export default TransferWindow;