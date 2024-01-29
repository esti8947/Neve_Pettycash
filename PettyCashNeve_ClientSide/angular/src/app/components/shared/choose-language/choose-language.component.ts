import { Inject, Renderer2 } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DOCUMENT } from '@angular/common';
import { AuthService } from 'src/app/services/auth-service/auth.service';

@Component({
  selector: 'choose-language',
  templateUrl: './choose-language.component.html',
  styleUrls: ['./choose-language.component.scss'],
})
export class ChooseLanguageComponent implements OnInit {
  selectedLanguage: string = 'en-US';
  constructor(
    private readonly translateService: TranslateService,
    @Inject(DOCUMENT) private readonly ducument: Document,
    private readonly renderer: Renderer2,
    private authService:AuthService,
  ) {}

  ngOnInit() {
    this.translateService.setDefaultLang(this.selectedLanguage);
    this.translateService.use(this.selectedLanguage);
  }

  changeLanguage(lang: string) {
    this.selectedLanguage = lang;
    this.translateService.setDefaultLang(lang);
    this.translateService.use(lang);    

    const body = this.ducument.querySelector('body');
    this.renderer.removeClass(body, 'hebrew-style');
    if (lang === 'he-IL') {
      this.renderer.addClass(body, 'hebrew-style');
    }
  }
}
