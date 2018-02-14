export interface NavMenuModel {
    pageTitle: string,
    links: NavMenuLinkModel[]
}

export interface NavMenuLinkModel {
    name: string,
    url: string
}