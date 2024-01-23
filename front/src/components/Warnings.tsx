import { useEffect, useState } from "react";
import { getactivities } from "../Client";
import { getCredentials } from "../utils/Cipher";
import "../styles/Warnings.css";


interface WarningsProps {
    logged: boolean;
}


function convert(code: number): string {
    switch (code){
        case 0:
            return "Login";
        case 1:
            return "Logout";
        case 2:
            return "Register";
        case 3:
            return "GetAccount";
        case 4:
            return "NewTransfer";
        case 5:
            return "ChangePassword";
        case 6:
            return "GetActivities";
        case 7:
            return "ChangePasswordCodeRequest";
        case 8:
            return "CodeSubmit";
        default:
            return "Unknown";
    }}

function Warnings(props: WarningsProps) {
    const {logged} = props;

    var credentials = getCredentials().email || "";
    const [activities, setActivities] = useState([]);

    useEffect(() => {
        getactivities(credentials as string)
            .then((response) => {
                console.log(response);
                setActivities(response);
            });
    }, [logged]);

    return ( logged ?
        <>
        <div className="warning-window">
                <div className="warning-table">
                <table>
                <tbody className="table-header">
                    <tr>
                        <th>
                            Date
                        </th>
                        <th>
                            Type
                        </th>
                        <th>
                            Origin
                        </th>
                    </tr>
                </tbody>
            </table>
                    <tbody >
                    {activities.map((activity: any) => {
                        return (
                            <tr>
                                <th>
                                    <p>{activity.date.replace('T', ' ').split('.')[0]}</p>
                                </th>
                                <th>
                                    <p>{convert(activity.type)}</p>
                                </th>
                                <th>
                                    <p>{activity.origin}</p>
                                </th>
                            </tr>
                        )
                    })}
                    </tbody>
                </div>
            </div>
        </> : <></>
    )

}

export default Warnings;