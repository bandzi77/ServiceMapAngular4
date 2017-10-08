export class IPage {
        totalCount: number;
        pageSize: number;       
}
export class IPageInfo {
    current_page?: number; 
    page_size: number;       
    order_by: string;
}