import './App.css'
import {useState} from 'react'
import Account from './components/Account'
import TransferWindow from './components/TransferWindow'
import AuthWindow from './components/AuthWindow';

function App() {
  const [twv, setTWV] = useState(false);
  const [log, setLog] = useState(false);

  return (
    <>
      <AuthWindow logged={log} setLogged={(e: boolean) => setLog(e)}/>
      <TransferWindow state={twv} close={() => setTWV(false)}/>
      <Account newTransfer={() => setTWV(true)} logOut={() => setLog(false)}/>
    </>
  )
}

export default App
