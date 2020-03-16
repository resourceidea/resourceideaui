import React, { Component } from "react";
import { Link, Route } from "react-router-dom";
import AddLineOfService from "./AddLineOfService";

class LinesOfService extends Component{
    render(){
        return(
            <div className='card text-justify'>
                <div className='card-header'>
                    Lines of service
                </div>
                <div className='card-body'>
                    <Link to='/admin/lines-of-service/add' className='btn btn-primary'>New line of service</Link>
                    <Route path='/admin/lines-of-service/add' component={AddLineOfService} />
                </div>
            </div>
        );
    }
}

export default LinesOfService;