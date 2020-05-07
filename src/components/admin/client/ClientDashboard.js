import React, { Component } from "react";
import { Link } from "react-router-dom";

class ClientDashboard extends Component{
    render(){
        return(
            <div>
                <div className='text-left'>
                    <Link 
                        to='/admin/clients' 
                        className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/clients/list'
                        className='btn btn-light btn-sm text-left'>Clients list</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/clients/add'
                        className='btn btn-light btn-sm text-left'>Add new client</Link>&nbsp;&nbsp;
                </div>
                <br />
                <div className='text-justify'>
                    <h5>Clients Dashboard</h5>
                    <hr />
                </div>
            </div>
        );
    }
}

export default ClientDashboard;