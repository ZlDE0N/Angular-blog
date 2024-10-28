import { Component, Input, OnInit } from '@angular/core';
import { Entry } from '../../../models/entry.model';
import { EntriesService } from '../../../services/entries.service';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.css'],
})
export class PostCardComponent implements OnInit {
  @Input() postData!: Entry;

  constructor(private entriesService: EntriesService, private router: Router){
  }

  ngOnInit(): void {
  }

  navigateToPost(postData: any) {
    this.entriesService.setPostData(postData); // Guarda los datos en el servicio
    this.router.navigate(['/post', postData.id]);
  }
}
