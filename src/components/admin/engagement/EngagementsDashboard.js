import React, { Component } from "react";
import { Link } from "react-router-dom";

class EngagementsDashboard extends Component{
    render(){
        return(
            <div>
                <div className='text-left'>
                    <Link 
                        to='/admin/engagements' 
                        className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/engagements/list'
                        className='btn btn-light btn-sm text-left'>Engagements list</Link>&nbsp;&nbsp;
                    <Link 
                        to='/admin/engagements/add'
                        className='btn btn-light btn-sm text-left'>Add new engagements</Link>&nbsp;&nbsp;
                </div>
                <br />
                <div className='text-justify'>
                    <h5>Engagements Dashboard</h5>
                    <hr />
                </div>
            </div>
        );
    }
}

export default EngagementsDashboard;