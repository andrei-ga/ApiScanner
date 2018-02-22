import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class PageHeaderService {
    public embed = new BehaviorSubject<boolean>(true);

    public setEmbed(value: boolean) {
        this.embed.next(value);
    }
}