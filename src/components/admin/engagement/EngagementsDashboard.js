import React, { Component } from "react";
import AdminActionBar from "../../common/actionbar/AdminDashboardActionBar";

class EngagementsDashboard extends Component {
    render() {
        return (
            <div>
                <div className='text-justify'>
                    <AdminActionBar
                        page='engagement'
                        dashboardLink='/admin/engagements' 
                        listLink='/admin/engagements/list'
                        addNewLink='/admin/engagements/add' />
                </div>
            </div>
        );
    }
}

export default EngagementsDashboard;