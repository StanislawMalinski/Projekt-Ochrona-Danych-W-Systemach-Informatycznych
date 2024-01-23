import './App.css'
import {useEffect, useState} from 'react'
import Account from './components/Account'
import TransferWindow from './components/TransferWindow'
import AuthWindow from './components/AuthWindow';
import { account, getPubKey } from './Client';
import { deleteCredentials, getCredentials } from './utils/Cipher';

const emptyAccount = { "accountNumber": "", "balance": 0, "history": [{ "accountNumber": "", "recipientAccountNumber": "", "recipient": "", "title": "", "value": 0 }] }

function App() {
  const [twv, setTWV] = useState(false);
  const [log, setLog] = useState(false);
  const [accountf, setAccountf] = useState(emptyAccount)
  
  useEffect(() => {
    getPubKey()
  }, [])

  const relod = () => {
    account({email: getCredentials().email})
    .then((response) => {
      if (response) {
        setAccountf(response);
      }
    })
  }

  return (
    <>
      <AuthWindow logged={log} setLogged={(e: boolean) => setLog(e)} setAccount={(e) => setAccountf(e)}/>
      <TransferWindow state={twv} close={() => setTWV(false)} accountNumber={accountf.accountNumber} relod={relod}/>
      <Account newTransfer={() => setTWV(true)} credentials={accountf} logOut={() => {setLog(false); deleteCredentials();}}/>
      
    </>
  )
}

export default App
//<Warnings logged={log}/>