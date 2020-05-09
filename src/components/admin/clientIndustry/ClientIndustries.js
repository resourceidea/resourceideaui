import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddClientIndustry from "./AddClientIndustry";

class ClientIndustries extends Component{
    render() {
        return(
            <div className="card text-justify">
                <div className='card-header'>
                    Client Industries
                </div>
                <div className="card-body">
                    <Link to='/admin/client-industries/add' className='btn btn-primary'>Add department</Link>
                    <Route path='/admin/client-industries/add' component={AddClientIndustry} />
                </div>
            </div>
        );
    }
}

export default ClientIndustries;